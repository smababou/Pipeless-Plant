%% RFID signal emulator
clear all
clc
close all
% Initializing
l1 = 100;   % length of the plant, x  [cm]
l2 = 100;   % width of the plant, y   [cm]
d1 = 20;    % distance between tags     [cm]
d2 = 0;     % distance last tag <-> boarder [cm]
r1 = 10;    % radius of the reading range of every tag
r2 = [r1, 9.5, 9.0, 8.0, 6.0, 5.0, 4.0]; % array with the different RSSI values

robStart = [25 25]; % Startposition of robot in x, y   [cm]
robEnd = [90 50];   % Endposition of robot in x, y     [cm]

robSpeed = 0.1;       % Speed robot [m/s]
cycleT = 100;        % Cycletime in [ms]

measuementeNumber = num2str(2); % Number of measurement

%% Error
if mod(l1/d1,1)~=0
    error('Length of platform not dividable by distance between tags');
elseif mod(l2/d1,1)~=0
    error('Length of platform not dividable by distance between tags');
end

%% Computing 
distance = pdist([robStart; robEnd] ,'euclidean');  % [cm]
t1 = (distance/100)/robSpeed;           % time to get to the goal[s]
angle = atan2(robEnd(2)-robStart(2),robEnd(1)-robStart(1));
distance2 = robSpeed * cycleT/10;                    % [cm]

numTagsX = (l1-2*d2)/d1 +1;
numTagsY = (l2-2*d2)/d1 +1;
numTags = numTagsX * numTagsY;

%% Display
d1_str = num2str(d1);
l1_str = num2str(l1);
l2_str = num2str(l2);
numTags_str = num2str(numTags);

msg0 = ['Your plane is ',l1_str,'cm x ',l2_str,'cm.'];
msg1 = ['You chose a distance of ',d1_str, 'cm and need ', numTags_str,' Tags!'];
disp(msg0);
disp(msg1);

%% Drawing
figure(1)
x1 = [0 l1 l1 0 0];
y1 = [0 0 l2 l2 0];
plot(x1, y1,'LineWidth',2)
xlim([-5 (l1+5)]);
ylim([-5 (l2+5)]);
hold on

% Position of the tags
ID = 1:numTags;
[Tagx,Tagy] = meshgrid(d2:d1:l1-d2,d2:d1:l2-d2);
plot(Tagx,Tagy,'r*')
% Circles
radiipl = ones(numTagsX,1)*r1;
for k=1:numTagsX
    tempx = Tagx(1:end,k);
    tempy = Tagy(1:end,k);
    temppos = horzcat(tempx,tempy);
    viscircles(temppos,radiipl,'Color','k','LineStyle',':','LineWidth',0.25);
end
robX = [robStart(1),robEnd(1)];
robY = [robStart(2),robEnd(2)];
plot(robX,robY,'bO','LineWidth',3);
plot(robX,robY,'r:');
xlabel('Length platform in cm')
ylabel('Width platform in cm')
title({'Position and reading range of tags';'Start-, endpoint and path of the robot'});
hold off
pause(1)
 
%% Animation and loggin
xUpdate = robStart(1);
yUpdate = robStart(2);
deltaX = distance2*cos(angle);
deltaY = distance2*sin(angle);
% Txt file name
name = ['Meas_new',measuementeNumber,'.txt'];
fileID = fopen(name,'w');
% fprintf(fileID,'%6s %12s %18s\n','Tag ID','Strength','Time');
% Data stored in variables
dataRSSI = zeros(length(0:cycleT/1000:t1),numTags);
streamDataRSSI = zeros(1,numTags);
streamDataRSSIold = zeros(1,numTags);
timeStep = 1;   % current time step

figure(2)
for l=0:cycleT/1000:t1                                
    % tic
    xUpdate = xUpdate + deltaX;
    yUpdate = yUpdate + deltaY;
    plot(x1, y1,'LineWidth',2)
    hold on
    xlim([-5 (l1+5)]);
    ylim([-5 (l2+5)]);
    [Tagx,Tagy] = meshgrid(d2:d1:l1-d2,d2:d1:l2-d2);
    plot(Tagx,Tagy,'r*')
    plot(robX,robY,'bO','LineWidth',1);
    plot(robX,robY,'r:');
    plot(xUpdate,yUpdate,'kO','LineWidth',2);
    xlim([-5 (l1+5)]);
    ylim([-5 (l2+5)]);
    hold off
    
    % Creating measurements
    robPos=[xUpdate,yUpdate];
    for m = 1:numTags                   % m = current number of tag
        m_str = num2str(m);
        tempTag=[Tagx(m),Tagy(m)];
        tempD = pdist([robPos; tempTag] ,'euclidean');
        % Display if Tag is in range or not
        if tempD > r1
               % disp(['Label ',m_str,' out of range.']);
               % fprintf(fileID,'%6d %12s %18.2f\n',m,'0',l);
               streamDataRSSI(m) = 0;
               if streamDataRSSI(m) ~= streamDataRSSIold(m)
                    fprintf(fileID,'%6d %12s %18.2f\n',m,'0',l);
                    fprintf('%6d %12s %18.2f\n',m,'0',l);
               end
        elseif tempD <= r1
                % disp(['Label ',m_str,' in range!!!!!!!!!!!!!']);
                % Relation distance <-> RSSI
                k = find(r2>tempD);
                % fprintf(fileID,'%6d %12d %18.2f\n',m,k(end),l);
                dataRSSI(timeStep,m) = k(end);  
                streamDataRSSI(m) = k(end);
                if streamDataRSSI(m) ~= streamDataRSSIold(m)
                    fprintf(fileID,'%6d %12d %18.2f\n',m,k(end),l);
                    fprintf('%6d %12d %18.2f\n',m,k(end),l);
               end
        end
    end
    streamDataRSSIold = streamDataRSSI;
    % fprintf(fileID,'%6s\n','---------');  
    % toc
    pause(cycleT/1000)
    timeStep = timeStep + 1;
end

fclose(fileID);

%% Results
figure(3)   % plot for the max value of every tag
dataRSSInoT = reshape(max(dataRSSI),[numTagsX,numTagsY]);
plot3(Tagx,Tagy,dataRSSInoT,'*');   
xlabel('Length platform in cm')
ylabel('Width platform in cm')
title('Max RSSI signal of every tag')

figure(4)   % plot of the RSSI signal which are non zero vs. time
dataRSSIsum = sum(dataRSSI);
IDclear = find(dataRSSIsum ~= 0);
IDstr = string(IDclear); 
dataRSSIclear = dataRSSI;
dataRSSIclear( :, all( ~any( dataRSSI ), 1 ) ) = []; % and columns
plot(dataRSSIclear);
xlabel('Time in 100 ms')
ylabel('RSSI')
ylim([0 8])
legend(IDstr,'FontSize',6);
title('RSSI Signal of every non zero tag')