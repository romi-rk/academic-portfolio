-- AMP Case Study 2026 - Group 30
-- Elliptical Gear set with axis in the focus point, with sine tooth profile
-- 1. HRITIK SURESH JAIN            (22403618)
-- 2. AJAY ARJUN MANNOLKAR          (22402834)
-- 3. SHUBHAM DNYANESHWAR URMODE    (22408646)
-- 4. PARV RAJESHKUMAR PARAKHIYA    (22408788)
-- 5. HARSHAL BOKIL                 (22409497)
-- 6. ROMIKUMAR RAMESHBHAI KUKADIYA (22409640)
-- 7. JAYDEV DAGA                   (22407988)

-- ===================================================
-- Section 1: User Inputs and Basic Gearbox Parameters
-- ===================================================

-- Parameters
local center_distance = ui_scalarBox("Center distance between shafts in mm", 80.0, 1)

local ratio_range = ui_scalarBox("Transmission ratio range (i_max/i_min)", 15, 0.1) -- i is Gear Ratio

if ratio_range <= 1 then
    print("ERROR: ratio range must be greater than 1 \n")
    ratio_range = 1.01
return
end

local phi_1_deg = math.floor(ui_scalar("Gear Animation Angle", 0, 0, 360))  -- Just to rotate Gears

-- Calculating ellipse parameters from gearbox parameters
local a_e = center_distance / 2

local epsilon_e = (math.sqrt(ratio_range) - 1) / (math.sqrt(ratio_range) + 1)

if epsilon_e <= 0 or epsilon_e >= 1 then
    print("ERROR: epsilon_e must be 0 < epsilon_e < 1")
    return
end

local b = tonumber(ui_numberBox("b - Facewidth / Gear Thickness in mm", 15.0))
local d_pin = tonumber(ui_numberBox("d_pin - Shaft hole diameter in mm", 5.0))
local c = ui_scalar("Clearance in mm", 0, 0, 1)  -- It is just to seperate Gears for Visual and Printing purpose


local alpha_0 = 22.5  -- Cutter Pressure Angle

local alpha_lim_deg = ui_scalar("alpha_s_lim - Sine flank slope angle limit in deg", 65, 60, 65)
--New Parameter, explanation of the same is given in the code line 118

local nPitch = 300  -- Resolution for the curve, plotting


-- ===============================================
-- Section 2: Ellipse Geometry and Reference Curve
-- ===============================================

-- Ellipse Geometry
local c_e = a_e * epsilon_e
local b_e = a_e * math.sqrt(1 - epsilon_e * epsilon_e)
local a = center_distance + c


-- Polar curve equation of Ellipse wrt Foci
local function r_p(phi)
    return a_e * (1 - epsilon_e * epsilon_e) /
           (1 - epsilon_e * math.cos(phi))
end


-- Calculating Perimeter using line Integral
local function calculatePerimeter()
    local x = {}
    local y = {}
    local s = {}

    for i = 1, nPitch do
        local phi = 2 * math.pi * (i - 1) / (nPitch - 1)
        local r_ref = r_p(phi)

        x[i] = r_ref * math.cos(phi)
        y[i] = r_ref * math.sin(phi)
    end

    s[1] = 0

-- Actual Line Integral Value
    for i = 2, nPitch do
        local dx = x[i] - x[i - 1]
        local dy = y[i] - y[i - 1]
        s[i] = s[i - 1] + math.sqrt(dx * dx + dy * dy)
    end

    return s[nPitch]
end

local L = calculatePerimeter()


-- ====================================================================
-- Section 3: Solving Interference and finding limit for m, z, and h_a
-- ====================================================================

-- Solving for module limits, z limits and h_max
local alpha_0_rad = math.rad(alpha_0)
local alpha_lim_rad = math.rad(alpha_lim_deg)

-- Radius of curvature for ellipse
local function rho(theta)
    return (2 * math.sqrt(2) * a_e * (1 - epsilon_e)) /
           math.pow(1 - epsilon_e * math.cos(theta), 1.5)
end

-- Minimum radius of curvature
local rho_min = rho(math.pi)

-- Height limit from curvature / undercut condition
local h_curvature_max = rho_min * math.sin(alpha_0_rad)^2


-- Sine flank slope angle
-- This is not the classical gear pressure angle, its a new parameter.
-- It is derived from the sine tooth profile, basically its slope at the pitch point:
-- y(s) = h_a * sin(2*pi*z*s/L)
-- dy/ds max = 2*pi*z*h_a/L (max will be when cos = 1, thetha = 0 or 180)
--tan(alpha_s) = 2*pi*z*h_a/L
-- alpha_s = atan(2*pi*z*h_a/L)
-- This angle is used only to monitor how steep the sine tooth flank becomes.
-- A very large alpha_s means steep tooth sides and higher risk of tooth intersection at higher number of teeth.
-- We can restrict the h_a_coeff based on this as follows,
-- Since L = pi*m*z and h_a = h_a_coeff*m,
-- alpha_s = atan(2*h_a_coeff)
local h_a_coeff_max_slope = math.tan(alpha_lim_rad) / 2

if h_a_coeff_max_slope > 1.25 then
    h_a_coeff_max_slope = 1.25
end

if h_a_coeff_max_slope < 0.1 then
    print("ERROR: alpha_s_lim too low for h_a_coeff range")
    return
end

local h_a_coeff_default = 1.0
if h_a_coeff_default > h_a_coeff_max_slope then
    h_a_coeff_default = h_a_coeff_max_slope
end

local h_a_coeff = ui_scalar("h_a* - Addendum coefficient", h_a_coeff_default, 0.1, h_a_coeff_max_slope)

-- Maximum module from h_a = h_a_coeff*m
local m_max = h_curvature_max / h_a_coeff

-- Minimum module for manufacturing and visibility
local m_min = 0.2  -- Research based value used for micro pitch gears used in samll instruments like wathces, prescision instruments

if m_max < m_min then
    print("ERROR: m_max smaller than m_min, not possible \n")
    return
end

-- Minimum and maximum teeth from L = pi*m*z
local z_min = math.ceil(L / (math.pi * m_max))

if z_min < 17 then
    z_min = 17
end

local z_max = math.floor(L / (math.pi * m_min)) --Deriving Zmax based of Minimum module

if z_max > 100 then --Setting a limit for visual and computing purpose
    z_max = 100
end

if z_max < z_min then
    print("ERROR: z_max smaller than z_min")
    print("Reduce m_min or increase center distance")
    return
end


-- Interdependent Module and Number of Teeth
-- Mode 1: module controls z
-- Mode 2: z controls module
local design_mode = ui_number("Design mode: 1 = module control, 2 = teeth control", 1, 1, 2)


local m_default = 1
local m_input = ui_scalar("m - Module in mm", m_default, m_min, m_max)

local z_input = ui_number("z - Number of teeth", z_min, z_min, z_max)

local m
local z

if design_mode == 1 then

    -- Module controls number of teeth
    m = m_input
    z = math.floor(L / (math.pi * m) + 0.5)

    if z < z_min then
        z = z_min
    end

    if z > z_max then
        z = z_max
    end

    -- Correcting actual module after integer z rounding
    m = L / (math.pi * z)

else

    -- Number of teeth controls module
    z = z_input
    m = L / (math.pi * z)

    if m < m_min then
        m = m_min
        z = math.floor(L / (math.pi * m) + 0.5)
    end

    if m > m_max then
        m = m_max
        z = math.floor(L / (math.pi * m) + 0.5)
    end
end

-- Actual addendum height
local h_a = h_a_coeff * m

-- Final maximum allowable height
local h_max = h_curvature_max

-- Sine flank slope angle
-- This is not the classical gear pressure angle.
-- It is derived from the sine tooth profile:
-- y(s) = h_a * sin(2*pi*z*s/L)
-- dy/ds max = 2*pi*z*h_a/L
-- alpha_s = atan(2*pi*z*h_a/L)
--alpha_s = atan(2*h_a_coeff)
-- This angle is used only to monitor how steep the sine tooth flank becomes.
-- A very large alpha_s means steep tooth sides and higher risk of tooth intersection.
local alpha_s_deg = math.deg(math.atan(2 * h_a_coeff))

-- Profile resolution after final z
local nProfile = z * 20


-- Setting a limit for shaft hole diameter
local r_min_focus = a_e * (1 - epsilon_e)

if d_pin > 2 * (r_min_focus - h_a - 1) then
    d_pin = 2 * (r_min_focus - h_a - 1)
end

if d_pin < 1 then
    d_pin = 1
end


-- =======================================================================
-- Section 4: Numerical Helper Functions for plotting Uniform pitch points
-- =======================================================================

-- Linear Interpolation for plotting uniform x,y on ellipse curve for teeth distribution according to the ellipse perimeter
local function interpolation(x_table, y_table, xq)
    local n = #x_table

    if xq <= x_table[1] then
        return y_table[1]
    end

    if xq >= x_table[n] then
        return y_table[n]
    end

    for i = 1, n - 1 do
        if xq >= x_table[i] and xq <= x_table[i + 1] then
            local t = (xq - x_table[i]) / (x_table[i + 1] - x_table[i])
            return y_table[i] * (1 - t) + y_table[i + 1] * t
        end
    end

    return y_table[n]
end


-- Output angle of Gear 2 based on V1 = V2 at contact point
-- This performs numerical integration of the transmission ratio to calculate the second gear's rotation angle
-- phi  - Angle of rotation of first gear
-- dphi - Small change in angle phi
-- psi  - Angle of rotation of the second gear
-- r1*(dphi_1) = r2*(dpsi_2)
-- r1 + r2 = center_distance
local function computeOutputAngle(phi_1_deg_in)
    local phi_1_target = math.rad(phi_1_deg_in)
    local dphi_1 = phi_1_target / nPitch
    local psi_2 = 0.0

    for i = 1, nPitch do
        local phi_1_mid = (i - 0.5) * dphi_1

        local r_w1 = r_p(phi_1_mid)
        local r_w2 = 2 * a_e - r_w1

        psi_2 = psi_2 + (r_w1 / r_w2) * dphi_1
    end

    return psi_2
end

local phi_1_rad = math.rad(phi_1_deg)
local psi_2_rad = computeOutputAngle(phi_1_deg)
local psi_2_deg = math.deg(psi_2_rad)


-- ========================================
-- Section 5: Gear Tooth Profile Generation
-- ========================================

-- Generating The Gear Profile
local function buildProfile(tooth_phase)

    tooth_phase = tooth_phase or 0

    local x = {}
    local y = {}
    local s = {}

-- Calculating x,y for the given ellipse profile
    for i = 1, nPitch do
        local phi = 2 * math.pi * (i - 1) / (nPitch - 1)
        local r_ref = r_p(phi)

        x[i] = r_ref * math.cos(phi)
        y[i] = r_ref * math.sin(phi)
    end

    s[1] = 0

-- Calculating the line Integral again, to find all cumulative s values
    for i = 2, nPitch do
        local dx = x[i] - x[i - 1]
        local dy = y[i] - y[i - 1]

        s[i] = s[i - 1] + math.sqrt(dx * dx + dy * dy)
    end

    local L_profile = s[nPitch]
    local pts = {}

-- Using interpolation function to plot the following uniform x,y points using uniform s points
    for i = 1, nProfile do
        local su = L_profile * (i - 1) / (nProfile - 1)

        local xu = interpolation(s, x, su)
        local yu = interpolation(s, y, su)

        local su_prev = math.max(0, su - L_profile / nProfile)
        local su_next = math.min(L_profile, su + L_profile / nProfile)

        local x_prev = interpolation(s, x, su_prev)
        local y_prev = interpolation(s, y, su_prev)

        local x_next = interpolation(s, x, su_next)
        local y_next = interpolation(s, y, su_next)

-- Using Interpolation points to create Local Tangent to plot Sine profile on Ellipse
        local dx = x_next - x_prev
        local dy = y_next - y_prev

        local mag = math.sqrt(dx * dx + dy * dy)

        local t_x
        local t_y

        if mag < 1e-9 then
            t_x = 1
            t_y = 0
        else
            t_x = dx / mag
            t_y = dy / mag
        end

        local n_x = t_y
        local n_y = -t_x

        local v_x = xu - c_e
        local v_y = yu

-- Using the dot product check to ensure, sine wave faces outside
        if n_x * v_x + n_y * v_y < 0 then
            n_x = -n_x
            n_y = -n_y
        end

-- Generating the sine Profile with a frequency equal to number of teeth
        local wave = math.sin(2 * math.pi * z * su / L_profile + tooth_phase)

        pts[i] = v(
            xu + h_a * wave * n_x,
            yu + h_a * wave * n_y,
            0
        )
    end

    pts[#pts + 1] = pts[1] -- End point is equal to start point to close the profile

    return pts
end


-- ==============================
-- Section 6: Gear Body Extrusion
-- ==============================

-- Extruding the Gear body
local function buildGear(tooth_phase)

    local profile = buildProfile(tooth_phase) -- Profile
    local body = linear_extrude(v(0, 0, b), profile) -- Extrude
    local shaft_hole = translate(v(0, 0, -1)) * cylinder(d_pin / 2, b + 2) -- Shaft hole

    return difference(body, shaft_hole)
end


-- ===============================================
-- Section 7: Gear Meshing, Mirroring and Rotating
-- ===============================================

-- For Even Number of teeth, phase angle is 0, and for odd, its pi
local gear2_phase = 0

if z % 2 == 0 then
    gear2_phase = 0
else
    gear2_phase = math.pi
end


-- Forming and Extruding Gears
local gear1_shape = buildGear(0)
local gear2_shape = buildGear(gear2_phase)

local gear1 = rotate(0, 0, phi_1_deg) * gear1_shape
local gear2 = translate(v(a, 0, 0)) * rotate(0, 0, -psi_2_deg) * gear2_shape
-- Used negative sign, since its an external gear.

emit(gear1, 1)
emit(gear2, 2)


-- =======================================================
-- Section 8: Printed Design Statistics for User Reference
-- =======================================================

-- Printing output for Statistics
local i_abs_max = (1 + epsilon_e) / (1 - epsilon_e)
local i_abs_min = (1 - epsilon_e) / (1 + epsilon_e)

print("Design Data\n")
print(string.format("Center distance = %.3f mm\n", center_distance))
print(string.format("Ratio range = %.3f\n", ratio_range))
print(string.format("epsilon_e = %.4f\n", epsilon_e))
print(string.format("rho_min = %.3f mm\n", rho_min))
print(string.format("h_max = %.3f mm\n", h_max))
print(string.format("h_a* = %.3f\n", h_a_coeff))
print(string.format("m_min = %.3f mm\n", m_min))
print(string.format("m_max = %.3f mm\n", m_max))
print(string.format("Selected / actual module m = %.3f mm\n", m))
print(string.format("Actual addendum height h_a = %.3f mm\n", h_a))
print(string.format("Height utilization h_a/h_max = %.3f\n", h_a / h_max))
print(string.format("z_min = %d\n", z_min))
print(string.format("z_max = %d\n", z_max))
print(string.format("Selected / calculated z = %d\n", z))
print(string.format("alpha_s = %.3f deg\n", alpha_s_deg))
print(string.format("|i|max = %.3f\n", i_abs_max))
print(string.format("|i|min = %.3f\n", i_abs_min))

