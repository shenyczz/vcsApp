PRO penmancal
  COMPILE_OPT idl2
  strtmp=''
  
  stainf=fltarr(4,179)
  
  metfile='E:\GYHY201406056\SHIYIDU\XUNDAN\MARdat4syd.txt'
  stafile='E:\GYHY201406056\SHIYIDU\XUNDAN\stainfo.txt'
  outfile='E:\GYHY201406056\SHIYIDU\XUNDAN\ET0_MAR.txt'
  
  openr,lun1,metfile,/get_lun
  openr,lun2,stafile,/get_lun
  openw,lun3,outfile,/get_lun,width=96108
  
  while (~eof(lun2)) do begin
    readf,lun2,stainf
  endwhile
  data=fltarr(10)
  PI=3.14159265
  readf,lun1,strtmp
  printf,lun3,'staid  lon   lat  alt  year  month  day  tmean  solarh  rain  et0'
  while (~eof(lun1)) do begin
    readf,lun1,staid,data
    ;dataҪ�أ�0�꣬1�£�2�գ�3TMEAN��4TMAX��5TMIN��6R_HUM��7SOLARH��8���٣�9RAIN��
    
    idx=where(stainf eq staid)
    lon=stainf[1,idx/4]
    lat=stainf[2,idx/4]
    alt=stainf[3,idx/4]
    jday=julday(data[1],data[2],2016)-julday(1,1,2016)
    lat_angl=lat*PI/180  ;γ�Ƚ�
    riqingjiao=0.409*sin(0.0172*jday-1.39)   ;�����
    rizhaoshishujiao=acos(-tan(lat_angl)*tan(riqingjiao))  ;����ʱ����
    ridijuli=1+0.033*cos(0.0172*jday)  ;���վ���
    daqidingfs=37.6*ridijuli*(rizhaoshishujiao*sin(lat_angl)*sin(riqingjiao)+cos(lat_angl)*cos(riqingjiao)*sin(rizhaoshishujiao))  ;�����㶥����
    kenengrizhao=7.64*rizhaoshishujiao   ;����������ʱ��
    changbofs=0.77*(0.25+0.5*data[7]/10/kenengrizhao)*daqidingfs  ;����������Rns
    dibiaofstl=(0.25+0.5*data[7]/10/kenengrizhao)*daqidingfs   ;������ͨ��
    qkduanbofs=(0.75+alt*10^(-5))*daqidingfs   ;��ն̲�����
    duanbofs=(1-0.23)*dibiaofstl   ;���̲�����Rns
    
    daqiya=101.3*((293-0.0065*alt)/293)^5.26   ;����ѹ
    zhengfaqr=2.501-data[3]/10*(2.361*10^(-3))    ;ˮ������Ǳ��
    ganshiwdjcs=0.00163*daqiya/zhengfaqr    ;��ʪ�¶ȼƳ���
    delta=4098*(0.6108*(2.718^((17.27*double(data[3]/10))/(data[3]/10+237.3))))/((data[3]/10+237.3)^2)  ;Delta************
    etmax=0.6108*2.718^((17.27*data[4]/10)/(data[4]/10+237.3))     ;eTmax
    etmin=0.6108*2.718^((17.27*data[5]/10)/(data[5]/10+237.3))     ;eTmin
    pjbaohesqy=(etmax+etmin)/2     ;ƽ������ˮ��ѹ
    shuiqiya=data[6]/1000*(pjbaohesqy)   ;ʵ��ˮ��ѹ
    fengsu2m=data[8]/10*(4.87/alog(67.8*10-5.42))   ;2�״�����
    
    jchangbofs=4.903*(((double(data[4])/10+273.16)^4+(double(data[5])/10+273.16)^4)/2)/10.0^(9)*(0.34-0.14*sqrt(shuiqiya))*(1.35*dibiaofstl/qkduanbofs-0.35)   ;����������Rn**********
    jfstl=duanbofs-jchangbofs   ;������ͨ��
    trrtl=0    ;������ͨ��
    dayet0=(0.408*delta*(jfstl-trrtl)+ganshiwdjcs*(900/(data[3]/10+273))*$
      fengsu2m*(pjbaohesqy-shuiqiya))/(delta+ganshiwdjcs*(1+0.34*fengsu2m))
    IF MAX(DATA[3:8]) GT 32000 THEN DAYET0=32700
    printf,lun3,string(long(staid))+string(format='(f7.2)',lon)+string(format='(f7.2)',lat)+string(format='(f9.1)',alt)$
      +string(fix(data[0]))+string(fix(data[1]))+string(fix(data[2]))+string(format='(f10.2)',data[3]/10)$
      +string(format='(f10.2)',data[7]/10)+string(format='(f10.1)',data[9]/10)+string(format='(d10.3)',dayet0)
  endwhile
  
  free_lun,lun1
  free_lun,lun2
  free_lun,lun3
  ;free_lun,lun4
END