using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Basler.Pylon;
//using Microsoft.VisualBasic.Logging;

namespace _NScrewMC_C_V1
{
    public class BaslerCam : ICameraInterface
    {
        public Camera camera = null;
        
        private Stopwatch stopWatch = new Stopwatch();

        //PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE();
        private bool isOpened = false;
        private PictureBox thisControl;

        public Bitmap Image_BASLER;
        public bool isgrabed = false;
        public bool isContinue = false;
        public string cameraName;
        public string deviceType;
        private PixelDataConverter converter = new PixelDataConverter();

        private int gain;
        bool EventHandler1 = false; // 이밴트 종료 핸들러
        public static readonly object _LockImage = new object();
        // Set up the controls and events to be used and update the device list.
        public Camera GetCamera()
        {
            return camera;
        }
        public override bool isContinuous()
        {
            if (isContinue == true)
                return true;
            return false;
        }
        public override void SetPictureBox(PictureBox control)
        {
            this.thisControl = control;
        }

        public BaslerCam(string userDefinedName)
        {
            MSystem.LoadParameter();
            InforManager.Instance.LoadSetting();
            if (InforManager.Instance.m_bVisionType)
            {
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                try
                {
                    foreach (ICameraInfo cameraInfo in allCameras)
                    {
                        //for Gig-E type Camera
                        //if (cameraInfo[CameraInfoKey.UserDefinedName] == userDefinedName)

                        // for USB type Camera
                        //if (cameraInfo[CameraInfoKey.FriendlyName].Contains(userDefinedName))

                        if (cameraInfo[CameraInfoKey.UserDefinedName] == userDefinedName)
                        {
                            string cameraSerial = cameraInfo[CameraInfoKey.SerialNumber];
                            // Create a new camera object.
                            camera = new Camera(cameraSerial);
                            cameraName = userDefinedName;
                            deviceType = cameraInfo[CameraInfoKey.DeviceType];

                            camera.CameraOpened += Configuration.AcquireContinuous;

                            // Register for the events of the image provider needed for proper operation.
                            camera.ConnectionLost += OnConnectionLost;
                            camera.CameraOpened += OnCameraOpened;
                            camera.CameraClosed += OnCameraClosed;
                            camera.StreamGrabber.GrabStarted += OnGrabStarted;
                            camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                            camera.StreamGrabber.GrabStopped += OnGrabStopped;

                            camera.Open();

                            isOpened = true;

                            // Load userset
                            camera.Parameters[PLCamera.UserSetLoad].Execute();

                            //camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                            //ContinuousShot();
                            //break;
                        }

                        #region past
                        //foreach (ICameraInfo cameraInfo in allCameras)
                        //{
                        //    // Create a new camera object.
                        //    camera = new Camera(allCameras[index]);
                        //    if (!camera.ToString().Contains(modelName))
                        //    {
                        //        index++;
                        //        camera = new Camera(allCameras[index]);
                        //    }
                        //}
                        #endregion
                        ////break;
                        //MSystem._mRun = new Thread(_UpdateImage);
                        //MSystem._mRun.IsBackground = true;
                        //MSystem._mRun.Start();

                    }
                }
                catch
                {
                    Thread.Sleep(300);
                    try
                    {
                        #region Skip
                        //camera.CameraOpened += Configuration.AcquireContinuous;
                        //// Register for the events of the image provider needed for proper operation.
                        //camera.ConnectionLost += OnConnectionLost;
                        //camera.CameraOpened += OnCameraOpened;
                        //camera.CameraClosed += OnCameraClosed;
                        //camera.StreamGrabber.GrabStarted += OnGrabStarted;
                        //camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                        //camera.StreamGrabber.GrabStopped += OnGrabStopped;
                        #endregion
                        camera.Open();
                    }
                    catch (Exception ex)
                    {
                        //Log.AddLog("BaslerCamera() in BaslerCamera exception " + ex.ToString());
                        Thread.Sleep(300);
                        #region Skip
                        //try
                        //{
                        //    LanCard("Camera", false);
                        //    LanCard("Camera", true);

                        //    Thread.Sleep(8000);

                        //    camera.CameraOpened += Configuration.AcquireContinuous;

                        //    // Register for the events of the image provider needed for proper operation.
                        //    camera.ConnectionLost += OnConnectionLost;
                        //    camera.CameraOpened += OnCameraOpened;
                        //    camera.CameraClosed += OnCameraClosed;
                        //    camera.StreamGrabber.GrabStarted += OnGrabStarted;
                        //    camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                        //    camera.StreamGrabber.GrabStopped += OnGrabStopped;

                        //    camera.Open();
                        //MessageBox.Show(e.ToString());
                        //}
                        //catch(Exception eee)
                        //{
                        //    m
                        //}
                        #endregion
                    }
                }
            }
        }
        public override void Init()
        {
            for (int i = 0; i < AoiParam.Instance.NumberOfCamera; i++)
            {
                if (AoiParam.Instance.AoiParams[i].CameraName == cameraName)
                {
                    CameraParams readJsonParam = new CameraParams();
                    readJsonParam.ExposureValue = AoiParam.Instance.AoiParams[i].Exposure;
                    readJsonParam.Width = AoiParam.Instance.AoiParams[i].SizeW;
                    readJsonParam.Height = AoiParam.Instance.AoiParams[i].SizeH;
                    readJsonParam.Xoffset = AoiParam.Instance.AoiParams[i].OffsetX;
                    readJsonParam.Yoffset = AoiParam.Instance.AoiParams[i].OffsetY;

                    SetParameter(readJsonParam);
                }
            }
        }
        public override void Init_SET1_LEFT()
        {
            if (AoiParam.Instance.SET1_LEFT == "SET1_LEFT")
            {
                //CameraParams readJsonParam = new CameraParams();
                //// 값을 넣어주는 부분, JSON으로 넣어준다.
                //readJsonParam.ExposureValue = AoiParam.Instance.Exposure;
                //readJsonParam.Width = AoiParam.Instance.SizeW;
                //readJsonParam.Height = AoiParam.Instance.SizeH;
                //readJsonParam.Xoffset = AoiParam.Instance.OffsetX;
                //readJsonParam.Yoffset = AoiParam.Instance.OffsetY;

                //// SetParameter에 삽입
                //SetParameter(readJsonParam);
            }

            //for (int i = 0; i < AoiParam.Instance.NumberOfCamera; i++)
            //{
            //    if (AoiParam.Instance.AoiParams[i].CameraName == cameraName)
            //    {
            //        CameraParams readJsonParam = new CameraParams();
            //        readJsonParam.ExposureValue = AoiParam.Instance.AoiParams[i].Exposure;
            //        readJsonParam.Width = AoiParam.Instance.AoiParams[i].SizeW;
            //        readJsonParam.Height = AoiParam.Instance.AoiParams[i].SizeH;
            //        readJsonParam.Xoffset = AoiParam.Instance.AoiParams[i].OffsetX;
            //        readJsonParam.Yoffset = AoiParam.Instance.AoiParams[i].OffsetY;

            //        SetParameter(readJsonParam);
            //    }
            //}
        }
        public override void Init_SET1_RIGHT()
        {
            for (int i = 0; i < AoiParam.Instance.NumberOfCamera; i++)
            {
                //if (AoiParam.Instance.AoiParams[i].CameraName == cameraName)
                //{
                //    CameraParams readJsonParam = new CameraParams();
                //    readJsonParam.ExposureValue = AoiParam.Instance.AoiParams[i].Exposure;
                //    readJsonParam.Width = AoiParam.Instance.AoiParams[i].SizeW;
                //    readJsonParam.Height = AoiParam.Instance.AoiParams[i].SizeH;
                //    readJsonParam.Xoffset = AoiParam.Instance.AoiParams[i].OffsetX;
                //    readJsonParam.Yoffset = AoiParam.Instance.AoiParams[i].OffsetY;

                //    SetParameter(readJsonParam);
                //}
            }
        }
        public override void Init_SET2_LEFT()
        {
            for (int i = 0; i < AoiParam.Instance.NumberOfCamera; i++)
            {
                //if (AoiParam.Instance.AoiParams[i].CameraName == cameraName)
                //{
                //    CameraParams readJsonParam = new CameraParams();
                //    readJsonParam.ExposureValue = AoiParam.Instance.AoiParams[i].Exposure;
                //    readJsonParam.Width = AoiParam.Instance.AoiParams[i].SizeW;
                //    readJsonParam.Height = AoiParam.Instance.AoiParams[i].SizeH;
                //    readJsonParam.Xoffset = AoiParam.Instance.AoiParams[i].OffsetX;
                //    readJsonParam.Yoffset = AoiParam.Instance.AoiParams[i].OffsetY;

                //    SetParameter(readJsonParam);
                //}
            }
        }
        public override void Init_SET2_RIGHT()
        {
            for (int i = 0; i < AoiParam.Instance.NumberOfCamera; i++)
            {
                //if (AoiParam.Instance.AoiParams[i].CameraName == cameraName)
                //{
                //    CameraParams readJsonParam = new CameraParams();
                //    readJsonParam.ExposureValue = AoiParam.Instance.AoiParams[i].Exposure;
                //    readJsonParam.Width = AoiParam.Instance.AoiParams[i].SizeW;
                //    readJsonParam.Height = AoiParam.Instance.AoiParams[i].SizeH;
                //    readJsonParam.Xoffset = AoiParam.Instance.AoiParams[i].OffsetX;
                //    readJsonParam.Yoffset = AoiParam.Instance.AoiParams[i].OffsetY;

                //    SetParameter(readJsonParam);
                //}
            }
        }
        public override bool IsOpened()
        {
            return isOpened;
        }
        public override void Start()
        {
            new NotImplementedException();
        }
        private void cmdProcess_Exited(object sender, System.EventArgs e)
        {
            EventHandler1 = true;   //외부 프로그램이 종료되면 이밴트 핸들러 전역변수를 true로 전환
        }
        public void LanCard(string 랜카드_이름, bool ON_OFF) //2015-04-24
        {
            string Alive = "disabled";
            if (ON_OFF == true)
            {
                Alive = "enabled";
            }

            System.Diagnostics.Process netsh = new System.Diagnostics.Process();
            netsh.StartInfo.FileName = "Netsh";

            if (랜카드_이름 != "")
            {
                try
                {
                    netsh.StartInfo.Arguments = "interface set interface name=" + '"' + 랜카드_이름 + '"' + " admin=" + Alive;
                    netsh.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    netsh.EnableRaisingEvents = true;
                    netsh.Exited += new EventHandler(cmdProcess_Exited);
                    EventHandler1 = false;
                    netsh.Start();
                    int script_run_time = 12000; //외부 프로그램의 최대 실행 시간 지정
                    int elapsedTime = 0;
                    const int SLEEP_AMOUNT = 100;
                    while (!EventHandler1)
                    {
                        elapsedTime += SLEEP_AMOUNT;
                        if (elapsedTime > script_run_time)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(SLEEP_AMOUNT);
                        //_Delay(SLEEP_AMOUNT);
                    }
                }
                catch
                {
                    return;
                }
            }
        }
        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            if (isContinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnConnectionLost), sender, e);
                    return;
                }
            }

            // Close the camera object.
            DestroyCamera();
        }

        // Occurs when the connection to a camera device is opened.
        private void OnCameraOpened(Object sender, EventArgs e)
        {
            if (isContinue)
            {

                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnCameraOpened), sender, e);
                    return;
                }
            }
        }

        // Occurs when the connection to a camera device is closed.
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            if (isContinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnCameraClosed), sender, e);
                    return;
                }
            }
        }

        // Occurs when a camera starts grabbing.
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            if (isContinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnGrabStarted), sender, e);
                    return;
                }
            }
            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.

            stopWatch.Reset();

        }
        public void DisplayImage()
        {

        }
        public override Bitmap OneShot_()
        {
            /******************************/
            //Reset Image sure for delete all
            if (Image_BASLER != null)
            {
                Image_BASLER.Dispose();
                //Image_BASLER = null;
                isgrabed = false;
            }
            /******************************/
            if (!OneShot())
            {
                if (Image_BASLER != null)
                {
                    Image_BASLER.Dispose();
                    isgrabed = false;
                }
                return Image_BASLER;
            }
            try
            {
                int i = 0;
                while (!isgrabed)
                {
                    i++;
                    Thread.Sleep(100);
                    if (i > 10)
                    {
                        MSystem.MyMessagerBottom($"{cameraName} Capture Fail");
                        if (Image_BASLER != null)
                        {
                            Image_BASLER.Dispose();
                            Image_BASLER = null;
                        }
                        return Image_BASLER;
                    }
                }
                return Image_BASLER;
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom($"{cameraName} Capture Fail");
            }
            return null;
        }
        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (isContinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                    thisControl.BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
                    return;
                }
            }
            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 100)
                    {
                        stopWatch.Restart();

                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                        bitmap.UnlockBits(bmpData);
                        // bitmap.Save(@".\Detail\Area\cameraraw1.bmp");

                        // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
                        if (isContinue)
                        {
                            Bitmap bitmapOld = ((PictureBox)thisControl).Image as Bitmap;
                            // Provide the display control with the new bitmap. This action automatically updates the display.
                            ((PictureBox)thisControl).Image = bitmap;
                            if (bitmapOld != null)
                            {
                                // Dispose the bitmap.
                                bitmapOld.Dispose();
                            }
                        }
                        else
                        {
                            if (bitmap != null)
                            {
                                Image_BASLER = bitmap;
                                isgrabed = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MSystem.MyMessagerBottom($"{cameraName} Take Image Fail");

            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
                GC.Collect();
            }
        }

        public void _UpdateImage()
        {
            while (true)
            {
                lock (_LockImage)
                {
                    try
                    {
                        if (camera != null && camera.IsOpen && camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
                        {
                            camera.ExecuteSoftwareTrigger();
                        }
                    }
                    catch (Exception)
                    {
                        MSystem.MyMessagerBottom("Triger Image Error");
                    }
                    if (!MSystem.m_bLife) break;
                }
                Thread.Sleep(1);
            }
        }
        // Occurs when a camera has stopped grabbing.
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            if (isContinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<GrabStopEventArgs>(OnGrabStopped), sender, e);
                    return;
                }
            }

            // Reset the stopwatch.
            stopWatch.Reset();

            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Stops the grabbing of images and handles exceptions.
        public override void Stop()
        {
            // Stop the grabbing.
            try
            {
                if (camera != null)
                    camera.StreamGrabber.Stop();
            }
            catch (Exception ex)
            {
                //Log.AddLog("Stop() in BaslerCamera exception " + ex.ToString());

            }
        }

        // Closes the camera object and handles exceptions.
        public override void DestroyCamera()
        {
            // Destroy the camera object.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch (Exception e)
            {
                //Log.AddLog(e.ToString());
            }
        }

        // Starts the grabbing of a single image and handles exceptions.
        public bool OneShot()
        {
            try
            {
                if (camera != null)
                {
                    if (!camera.IsOpen)
                    {
                        Thread.Sleep(100);
                        camera.Open();
                        //Thread.Sleep(500);
                    }

                    if (Image_BASLER != null)
                    {
                        Image_BASLER.Dispose();
                        Image_BASLER = null;
                    }
                    isgrabed = false;

                    camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame); // SingleFrame
                    camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    //GC.Collect();
                    return true;
                }
                else
                {
                    //Thread.Sleep(1000);
                    if (camera != null && !camera.IsOpen)
                    {
                        //camera.Close();
                        Thread.Sleep(100);
                        camera.Open();
                    }


                    isgrabed = false;
                    if (Image_BASLER != null)
                    {
                        Image_BASLER.Dispose();
                        Image_BASLER = null;
                    }

                    camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame); // SingleFrame
                    camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    //GC.Collect();
                    return true;
                }
            }
            catch (Exception)
            {
                Thread.Sleep(100);
                MSystem.MyMessagerBottom("Grap Camera Fail");
                //GC.Collect();
                return false;
            }
        }

        // Starts the continuous grabbing of images and handles exceptions.
        public void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                //camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                isgrabed = false;
                Image_BASLER = null;
            }
            catch (Exception)
            {

                //Log.AddLog("ContinuousShot() in BaslerCamera exception " + ex.ToString());
            }
        }

        public override CameraParams GetParameter()
        {
            cameraParams.ExposureValue = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
            cameraParams.MinExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
            cameraParams.MaxExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
            cameraParams.Width = (int)camera.Parameters[PLCamera.Width].GetValue();
            cameraParams.MinWidth = (int)camera.Parameters[PLCamera.Width].GetMinimum();
            cameraParams.MaxWidth = (int)camera.Parameters[PLCamera.Width].GetMaximum();
            cameraParams.Height = (int)camera.Parameters[PLCamera.Height].GetValue();
            cameraParams.MinHeight = (int)camera.Parameters[PLCamera.Height].GetMinimum();
            cameraParams.MaxHeight = (int)camera.Parameters[PLCamera.Height].GetMaximum();
            cameraParams.Xoffset = (int)camera.Parameters[PLCamera.OffsetX].GetValue();
            //cameraParams.MinXoff = (int)camera.Parameters[PLCamera.OffsetX].GetMinimum();
            //cameraParams.MaxXoff = (int)camera.Parameters[PLCamera.OffsetX].GetMaximum();
            cameraParams.Yoffset = (int)camera.Parameters[PLCamera.OffsetY].GetValue();
            //cameraParams.MinYoff = (int)camera.Parameters[PLCamera.OffsetY].GetMinimum();
            //cameraParams.MaxYoff = (int)camera.Parameters[PLCamera.OffsetY].GetMaximum();

            return cameraParams;
        }

        public override bool GetGammaStatus()
        {
            return camera.Parameters[PLCamera.GammaEnable].GetValue();
        }
        public override void SetGammaValue(double value)
        {
            camera.Parameters[PLCamera.Gamma].SetValue(value);
        }
        public override void SetGammaMode()
        {
            camera.Parameters[PLCamera.GammaEnable].SetValue(true);
        }
        public override void StartAutoExposure()
        {
            camera.Parameters[PLCamera.ExposureAuto].SetValue("Once");
        }

        public override void StopAutoExposure()
        {
            return;
        }

        public override int GetExposure()
        {
            return (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
        }

        public void WBAuto()
        {
            camera.Parameters[PLCamera.LightSourceSelector].SetValue("Off");
            camera.Parameters[PLCamera.BalanceWhiteAuto].SetValue("Once");
            Thread.Sleep(1000);
            camera.Parameters[PLCamera.BalanceWhiteAuto].SetValue("Off");
        }
        //public long GetColorBalance(AverageColor clr)
        //{
        //    switch (clr)
        //    {
        //        case AverageColor.NONE:
        //            break;
        //        case AverageColor.BLUE:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Blue");
        //            break;
        //        case AverageColor.GREEN:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Green");
        //            break;
        //        case AverageColor.RED:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Red");
        //            break;
        //        case AverageColor.WHITE:
        //            break;
        //        default:
        //            break;
        //    }
        //    return camera.Parameters[PLCamera.BalanceRatioRaw].GetValue();
        //}

        //public bool SetColorBalance(AverageColor clr, long value)
        //{
        //    switch (clr)
        //    {
        //        case AverageColor.NONE:
        //            break;
        //        case AverageColor.BLUE:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Blue");
        //            break;
        //        case AverageColor.GREEN:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Green");
        //            break;
        //        case AverageColor.RED:
        //            camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Red");
        //            break;
        //        case AverageColor.WHITE:
        //            break;
        //        default:
        //            break;
        //    }
        //    camera.Parameters[PLCamera.BalanceRatioRaw].SetValue(value);
        //    return true;
        //}

        public override void SetLivePlay(bool isLiveMode)
        {
            if (isLiveMode)
                isContinue = true;
            else
                isContinue = false;
        }

        public override bool GetLivePlay()
        {
            return isContinue;
        }

        public override bool SetParameter(CameraParams cameraParams)
        {
            try
            {
                camera.Parameters[PLCamera.Width].SetValue(cameraParams.Width, IntegerValueCorrection.Nearest);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.Height].SetValue(cameraParams.Height, IntegerValueCorrection.Nearest);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.OffsetX].SetValue(cameraParams.Xoffset, IntegerValueCorrection.Nearest);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.OffsetY].SetValue(cameraParams.Yoffset, IntegerValueCorrection.Nearest);
                Thread.Sleep(50);
                return true;
            }
            catch (Exception e)
            {
                //Log.AddLog(e.ToString());
                return false;
            }
        }
        public bool SetROIDefault()
        {
            try
            {
                camera.Parameters[PLCamera.OffsetX].TrySetToMinimum();
                Thread.Sleep(50);
                camera.Parameters[PLCamera.OffsetY].TrySetToMinimum();
                Thread.Sleep(50);
                camera.Parameters[PLCamera.Width].TrySetToMaximum();
                Thread.Sleep(50);
                camera.Parameters[PLCamera.Height].TrySetToMaximum();
                Thread.Sleep(50);
                return true;
            }
            catch (Exception)
            {
                //Log.AddLog(e.ToString());
                return false;
            }
        }
        public void InitCamFOV()
        {
            camera.Parameters[PLCamera.OffsetX].TrySetValue(0);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.OffsetY].TrySetValue(0);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.WidthMax].TrySetValue(4608);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.HeightMax].TrySetValue(3288);
            Thread.Sleep(50);
        }
        public override bool SetExposure(int exposure)
        {
            return camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(exposure, IntegerValueCorrection.Nearest);
        }

        public void SetGainValue(int _gain)
        {
            gain = _gain;
            camera.Parameters[PLCamera.GainRaw].TrySetValue(gain, IntegerValueCorrection.Nearest);
        }



        public override Bitmap OneShot_(int exposure)
        {
            return OneShot_();
        }


        public void WBAutoWithTargetValue(int targetValue)
        {
            throw new NotImplementedException();
        }

        public bool InitCamera(uint imiPixelFormat_BayerGR8)
        {
            throw new NotImplementedException();
        }

        public bool PreSetForColorTriggerShot()
        {
            throw new NotImplementedException();
        }

        public override bool SetTriggerMode(IntPtr _hDisplayWnd, IntPtr _Handle)
        {
            return true;
        }

        public override bool SetPreviewMode(IntPtr _hDisplayWnd, IntPtr _Handle)
        {
            SetLivePlay(true);
            ContinuousShot();
            return true;
        }

        public override void DisablePreviewMode(IntPtr _hDisplayWnd, IntPtr _Handle)
        {
            SetLivePlay(false);
            Stop();
        }

        public uint GetWBTargetValue()
        {
            throw new NotImplementedException();
        }

        public string GetFirmwareVersion()
        {
            throw new NotImplementedException();
        }
    }
}
