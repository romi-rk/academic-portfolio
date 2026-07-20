% Parameters
fs = 44100;            % Sampling frequency (Hz)
T = 0.5;                 
t = linspace(0, T, fs*T); 
f = 1000;              % Sound frequency (Hz)
P0 = 1;                % Sound pressure amplitude (Pa)

% Physical properties
B = 0.5;    % Magnetic flux density (Tesla)
L = 0.1;    % Length of coil wire (meters)
m = 0.01;   % Diaphragm mass (kg)
k = 1000;   % Spring constant (N/m)

% Noise is added to the input sound wave to make it realistic
noise_amplitude = 0.2;     

% Input sound pressure (sinusoidal wave with noise)
P = P0 * sin(2 * pi * f * t) + noise_amplitude * randn(size(t));

% Displacement and velocity of the diaphragm
omega_n = sqrt(k/m);   % Natural frequency
x = (P / k) .* (1 - cos(omega_n * t)); % Displacement
v = gradient(x, t(2) - t(1));          % Velocity (numerical derivative)

% Output voltage
V = B * L * v;

% Figure setup for the plots
figure;
set(gcf, 'Position', [100, 100, 800, 600]);

% Creating subplots for animation

% Input sound wave
subplot(3, 1, 1);
h_pressure = plot(t(1), P(1), 'b', 'LineWidth', 2);
title('Input Sound Pressure (with Noise)');
xlabel('Time (s)');
ylabel('Pressure (Pa)');
grid on;
ylim([-1.5*(P0 + noise_amplitude), 1.5*(P0 + noise_amplitude)]);

% Diaphragm displacement
subplot(3, 1, 2);
h_displacement = plot(0, 0, 'r', 'LineWidth', 2);
hold on;
diaphragm = plot(0, 0, 'ko', 'MarkerSize', 8, 'MarkerFaceColor', 'k');
title('Diaphragm Displacement');
xlabel('Time (s)');
ylabel('Displacement (m)');
grid on;
xlim([0, T]);
ylim([-max(x)*1.5, max(x)*1.5]);

% Output voltage 
subplot(3, 1, 3);
h_voltage = plot(t(1), V(1), 'g', 'LineWidth', 2);
title('Output Voltage');
xlabel('Time (s)');
ylabel('Voltage (V)');
grid on;
ylim([-1.5*max(V), 1.5*max(V)]);

n_frames = 1000;
step = round(length(t) / n_frames); 

for i = 1:step:length(t)

    set(h_pressure, 'XData', t(1:i), 'YData', P(1:i));
    
    set(h_displacement, 'XData', t(1:i), 'YData', x(1:i));
    set(diaphragm, 'XData', t(i), 'YData', x(i));
    
    set(h_voltage, 'XData', t(1:i), 'YData', V(1:i));
    pause(0.01);
end
