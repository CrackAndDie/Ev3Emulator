using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Cmemory.Interfaces;
using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Ccom
{
    public unsafe class Com_ : ICom
    {
        UWORD UsbConUpdate = 0;               // Timing of USB device side cable detection;
        UBYTE cComUsbDeviceConnected = 0;

        public RESULT cComInit()
        {
            RESULT Result = RESULT.FAIL;
            UWORD TmpFileHandle;
            UBYTE Cnt;

            GH.ComInstance.CommandReady = 0;
            // GH.ComInstance.Cmdfd = open(COM_CMD_DEVICE_NAME, O_RDWR, 0666); // TODO: do i need usb?

            if (GH.ComInstance.Cmdfd >= 0)
            {
                CommonHelper.memset(GH.ComInstance.TxBuf[0].Buf, 0, 1024);

                Result = OK;
            }
            for (TmpFileHandle = 0; TmpFileHandle < MAX_FILE_HANDLES; TmpFileHandle++)
            {
                GH.ComInstance.Files[TmpFileHandle].State = FS_IDLE;
                GH.ComInstance.Files[TmpFileHandle].Name[0] = 0;
                GH.ComInstance.Files[TmpFileHandle].File = -1;
            }

            for (Cnt = 0; Cnt < NO_OF_CHS; Cnt++)
            {
                GH.ComInstance.TxBuf[Cnt].BufSize = 1024;
                GH.ComInstance.TxBuf[Cnt].Writing = 0;
                GH.ComInstance.RxBuf[Cnt].BufSize = 1024;
                GH.ComInstance.RxBuf[Cnt].State = RXIDLE;
            }

            GH.ComInstance.ReadChannel[0] = cComReadBuffer;
            GH.ComInstance.ReadChannel[1] = null;
            GH.ComInstance.ReadChannel[2] = GH.Bt.cBtReadCh0;
            GH.ComInstance.ReadChannel[3] = GH.Bt.cBtReadCh1;
            GH.ComInstance.ReadChannel[4] = GH.Bt.cBtReadCh2;
            GH.ComInstance.ReadChannel[5] = GH.Bt.cBtReadCh3;
            GH.ComInstance.ReadChannel[6] = GH.Bt.cBtReadCh4;
            GH.ComInstance.ReadChannel[7] = GH.Bt.cBtReadCh5;
            GH.ComInstance.ReadChannel[8] = GH.Bt.cBtReadCh6;
            GH.ComInstance.ReadChannel[9] = GH.Bt.cBtReadCh7;
            GH.ComInstance.ReadChannel[10] = GH.Wifi.cWiFiReadTcp;

            GH.ComInstance.WriteChannel[0] = cComWriteBuffer;
            GH.ComInstance.WriteChannel[1] = null;
            GH.ComInstance.WriteChannel[2] = GH.Bt.cBtDevWriteBuf;
            GH.ComInstance.WriteChannel[3] = GH.Bt.cBtDevWriteBuf1;
            GH.ComInstance.WriteChannel[4] = GH.Bt.cBtDevWriteBuf2;
            GH.ComInstance.WriteChannel[5] = GH.Bt.cBtDevWriteBuf3;
            GH.ComInstance.WriteChannel[6] = GH.Bt.cBtDevWriteBuf4;
            GH.ComInstance.WriteChannel[7] = GH.Bt.cBtDevWriteBuf5;
            GH.ComInstance.WriteChannel[8] = GH.Bt.cBtDevWriteBuf6;
            GH.ComInstance.WriteChannel[9] = GH.Bt.cBtDevWriteBuf7;
            GH.ComInstance.WriteChannel[10] = GH.Wifi.cWiFiWriteTcp;

            for (Cnt = 0; Cnt < NO_OF_MAILBOXES; Cnt++)
            {
                GH.ComInstance.MailBox[Cnt].Status = (byte)RESULT.FAIL;
                GH.ComInstance.MailBox[Cnt].DataSize = 0;
                GH.ComInstance.MailBox[Cnt].ReadCnt = 0;
                GH.ComInstance.MailBox[Cnt].WriteCnt = 0;
                GH.ComInstance.MailBox[Cnt].Name[0] = 0;
            }

            GH.ComInstance.ComResult = OK;

            GH.ComInstance.BrickName[0] = 0;
            using FileStream fs = File.OpenRead("./Resources/other/BrickName"); 
            if (fs != null)
            {
                fs.ReadUnsafe((UBYTE*)&(GH.ComInstance.BrickName[0]), 0, (int)vmBRICKNAMESIZE);

                fs.Close();
            }

            // USB cable stuff init
            UsbConUpdate = 0;               // Reset timing of USB device side cable detection;
            cComUsbDeviceConnected = 0; // Until we believe in something else ;-)
            cComSetMusbHdrcMode();

            GH.Bt.BtInit((DATA8*)&(GH.ComInstance.BrickName[0]));
            GH.Daisy.cDaisyInit();
            GH.Wifi.cWiFiInit();

            GH.ComInstance.VmReady = 1;
            GH.ComInstance.ReplyStatus = 0;

            Result = OK;


            return (Result);

        }


        public RESULT cComOpen()
        {
            RESULT Result = RESULT.FAIL;

            Result = OK;

            return (Result);
        }


        public RESULT cComClose()
        {
            RESULT Result = RESULT.FAIL;
            UBYTE Cnt;

            for (Cnt = 0; Cnt < NO_OF_MAILBOXES; Cnt++)
            {
                GH.ComInstance.MailBox[Cnt].Status = (byte)RESULT.FAIL;
                GH.ComInstance.MailBox[Cnt].DataSize = 0;
                GH.ComInstance.MailBox[Cnt].ReadCnt = 0;
                GH.ComInstance.MailBox[Cnt].WriteCnt = 0;
                GH.ComInstance.MailBox[Cnt].Name[0] = 0;
            }

            Result = OK;

            return (Result);
        }


        public RESULT cComExit()
        {
            RESULT Result = RESULT.FAIL;

			// close(GH.ComInstance.Cmdfd); // TODO: do i need usb?

			Result = OK;

            GH.Wifi.cWiFiExit();
            GH.Bt.BtExit();

            return (Result);
        }

        // DAISY chain
        // Write type data to chain
        public RESULT cComSetDeviceInfo(DATA8 Length, UBYTE* pInfo)
        {
            return GH.Daisy.cDaisySetDeviceInfo(Length, pInfo);
            //return RESULT.FAIL;
        }

        // Read type data from chain
        public RESULT cComGetDeviceInfo(DATA8 Length, UBYTE* pInfo)
        {
            return GH.Daisy.cDaisyGetDeviceInfo(Length, pInfo);
        }

        // Write mode to chain
        public RESULT cComSetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode)
        {
            return GH.Daisy.cDaisySetDeviceType(Layer, Port, Type, Mode);
        }

        // Read device data from chain
        public RESULT cComGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData)
        {
            return GH.Daisy.cDaisyGetDownstreamData(Layer, Port, Length, pType, pMode, pData);
        }

        public void cComSetMusbHdrcMode()
        {
            // TODO: wtf ahahah

            //// Force OTG into mode
            //DATA8 Musb_Cmd[64];
            //// Build the command string for setting the OTG mode
            //strcpy(Musb_Cmd, "echo otg > /sys/devices/platform/musb_hdrc/mode");
            //system(Musb_Cmd);
        }

        public UBYTE cComCheckUsbCable()
        {
            // Get mode from MUSB_HDRC
            UBYTE Result = 0;

            // TODO: do i need usb?

            //DATA8 buffer[21];
            //FILE* f;

            //f = fopen("/sys/devices/platform/musb_hdrc/mode", "r");
            //if (f)
            //{
            //    fread(buffer, 20, 1, f);
            //    fclose(f);
            //}

            //GH.printf("BUFFER = %s\n\r", buffer);


            //if (strstr(buffer, "b_peripheral") != 0)
            //    Result = 1;

            if (Result == 1)
                GH.printf("CABLE connected\n\r");
            else
                GH.printf("CABLE dis-connected :-(\n\r");

            return Result;
        }

        public void cComShow(UBYTE* pB)
        {
            UWORD Lng;

            Lng = (UWORD)pB[0];
            Lng += (ushort)((UWORD)pB[1] << 8);

            GH.printf($"[{pB[0]}{pB[1]}");
            pB++;
            pB++;

            while (Lng != 0)
            {
                GH.printf($"{*pB}");
                pB++;
                Lng--;
            }
            GH.printf("]\r\n");
        }

        public void cComPrintTxMsg(TXBUF* pTxBuf)
        {
            ULONG Cnt, Cnt2;
            COMRPL* pComRpl;

            pComRpl = (COMRPL*)pTxBuf->Buf;

            GH.printf("Tx Buf content: \r\n");
            for (Cnt = 0; ((Cnt < ((*pComRpl).CmdSize + 2)) && (Cnt < 1024)); Cnt++)
            {
                for (Cnt2 = Cnt; Cnt2 < (Cnt + 16); Cnt2++)
                {
                    GH.printf($"0x{((UBYTE*)(&((*pComRpl).CmdSize)))[Cnt2]}, ");
                }
                Cnt = (Cnt2 - 1);
                GH.printf("\r\n");
            }
            GH.printf("\r\n");
        }

        public UWORD cComReadBuffer(UBYTE* pBuffer, UWORD Size)
        {
            UWORD Length = 0;

            // TODO: do i need usb?

            //// struct  timeval Cmdtv;
            //fd_set Cmdfds;

            //Cmdtv.tv_sec = 0;
            //Cmdtv.tv_usec = 0;
            //FD_ZERO(&Cmdfds);
            //FD_SET(GH.ComInstance.Cmdfd, &Cmdfds);
            //if (select(GH.ComInstance.Cmdfd + 1, &Cmdfds, null, null, &Cmdtv))
            //{
            //    Length = read(GH.ComInstance.Cmdfd, pBuffer, (size_t)Size);

            //    GH.printf("cComReadBuffer Length = %d\r\n", Length);
            //}
            return (Length);
        }


        public UWORD cComWriteBuffer(UBYTE* pBuffer, UWORD Size)
        {
            UWORD Length = 0;

            // TODO: do i need usb?

            //if (FULL_SPEED == GH.Daisy.cDaisyGetUsbUpStreamSpeed())
            //{
            //    Length = write(GH.ComInstance.Cmdfd, pBuffer, 64);
            //}
            //else
            //    Length = write(GH.ComInstance.Cmdfd, pBuffer, 1024);

            //GH.printf("cComWriteBuffer %d\n\r", Length);

            return (Length);
        }


        public UBYTE cComDirectCommand(UBYTE* pBuffer, UBYTE* pReply)
        {
            UBYTE Result = 0;
            COMCMD* pComCmd;
            COMRPL* pComRpl;
            DIRCMD* pDirCmd;
            CMDSIZE CmdSize;
            CMDSIZE HeadSize;
            UWORD Tmp;
            UWORD Globals;
            UWORD Locals;
            IMGHEAD* pImgHead;
            OBJHEAD* pObjHead;
            CMDSIZE Length;

            GH.ComInstance.VmReady = 0;
            pComCmd = (COMCMD*)pBuffer;
            pDirCmd = (DIRCMD*)(*pComCmd).PayLoad;

            CmdSize = (*pComCmd).CmdSize;
            HeadSize = (ushort)(((*pDirCmd).Code - pBuffer) - sizeof(CMDSIZE));
            Length = (ushort)(CmdSize - HeadSize);

            pComRpl = (COMRPL*)pReply;
            (*pComRpl).CmdSize = 3;
            (*pComRpl).MsgCnt = (*pComCmd).MsgCnt;
            (*pComRpl).Cmd = DIRECT_REPLY_ERROR;

            if ((CmdSize > HeadSize) && ((CmdSize - HeadSize) < (COM_GLOBALS.SizeofImage - (sizeof(IMGHEAD) + sizeof(OBJHEAD)))))
            {

                Tmp = (ushort)((UWORD)(*pDirCmd).Globals + ((UWORD)(*pDirCmd).Locals << 8));

                Globals = ((ushort)(Tmp & 0x3FF));
                Locals = ((ushort)((Tmp >> 10) & 0x3F));

                if ((Locals <= MAX_COMMAND_LOCALS) && (Globals <= MAX_COMMAND_GLOBALS))
                {

                    pImgHead = (IMGHEAD*)GH.ComInstance.Image;
                    pObjHead = (OBJHEAD*)(GH.ComInstance.Image + sizeof(IMGHEAD));

                    (*pImgHead).Sign[0] = (byte)'l';
                    (*pImgHead).Sign[1] = (byte)'e';
                    (*pImgHead).Sign[2] = (byte)'g';
                    (*pImgHead).Sign[3] = (byte)'o';
                    (*pImgHead).VersionInfo = (UWORD)(VERS * 100.0);
                    (*pImgHead).NumberOfObjects = (OBJID)1;
                    (*pImgHead).GlobalBytes = (GBINDEX)Globals;

                    (*pObjHead).OffsetToInstructions = (IP)(sizeof(IMGHEAD) + sizeof(OBJHEAD));
                    (*pObjHead).OwnerObjectId = 0;
                    (*pObjHead).TriggerCount = 0;
                    (*pObjHead).LocalBytes = (OBJID)Locals;

                    CommonHelper.memcpy(&GH.ComInstance.Image[sizeof(IMGHEAD) + sizeof(OBJHEAD)], (*pDirCmd).Code, Length);
					Length += (ushort)(sizeof(IMGHEAD) + sizeof(OBJHEAD));

                    GH.ComInstance.Image[Length] = opOBJECT_END;
                    (*pImgHead).ImageSize = Length;

                    //#define DEBUG

                    GH.printf("\r\n");
                    for (Tmp = 0; Tmp <= Length; Tmp++)
                    {
                        GH.printf($"{GH.ComInstance.Image[Tmp]} ");
                        if ((Tmp & 0x0F) == 0x0F)
                        {
                            GH.printf("\r\n");
                        }
                    }
                    GH.printf("\r\n");

                    Result = 1;
                }
                else
                {
                    (*pComRpl).Cmd = DIRECT_REPLY_ERROR;
                }
            }
            else
            {
                (*pComRpl).Cmd = DIRECT_REPLY_ERROR;
            }

            return (Result);
        }

        [Obsolete("Suck some dick")]
        public void cComCloseFileHandle(SLONG* pHandle)
        {
            if (*pHandle >= MIN_HANDLE)
            {
                // TODO: anime
                // close(*pHandle);
                *pHandle = -1;
            }
        }


        public UBYTE cComFreeHandle(DATA8 Handle)
        {
            UBYTE RtnVal = 1;

            if (0 != GH.ComInstance.Files[Handle].Name[0])
            {
                if ((Handle >= 0) && (Handle < MAX_FILE_HANDLES))
                {
                    GH.ComInstance.Files[Handle].Name[0] = 0;
                }
            }
            else
            {
                // Handle is unused
                RtnVal = 0;
            }
            return (RtnVal);
        }


        public DATA8 cComGetHandle(DATA8* pName)
        {
            DATA8 Result = 0;

            while ((GH.ComInstance.Files[Result].Name[0] != 0) && (Result < MAX_FILE_HANDLES))
            {
                Result++;
            }

            if (Result < MAX_FILE_HANDLES)
            {
                if (OK == GH.Memory.cMemoryCheckFilename(pName, null, null, null))
                {
                    CommonHelper.sprintf(GH.ComInstance.Files[Result].Name, CommonHelper.GetString((DATA8*)pName));
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                Result = -1;
            }
            return (Result);
        }


        public UBYTE cComGetNxtFile(DATA8* pDir, UBYTE* pName)
        {
            UBYTE RtnVal = 0;
            // struct dirent   * pDirPtr;

            DirectoryInfo di = new DirectoryInfo(CommonHelper.GetString(pDir));
            var files = di.GetFiles("*", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                CommonHelper.snprintf((DATA8*)pName, FILENAME_SIZE, files.First().Name);
                RtnVal = 1;
            }
            else
            {
            }

            return (RtnVal);
        }


        public void cComCreateBeginDl(TXBUF* pTxBuf, UBYTE* pName)
        {
            UWORD Index;
            UWORD ReadBytes;
            BEGIN_DL* pDlMsg;
            SBYTE FileHandle;
            DATA8* TmpFileName = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

            FileHandle = -1;
            pDlMsg = (BEGIN_DL*)&(pTxBuf->Buf[0]);

            if ((CommonHelper.strlen((DATA8*)pTxBuf->Folder) + CommonHelper.strlen((DATA8*)pName) + 1) <= vmFILENAMESIZE)
            {
                CommonHelper.sprintf(TmpFileName, CommonHelper.GetString((DATA8*)pTxBuf->Folder));
                CommonHelper.strcat(TmpFileName, (DATA8*)pName);

                FileHandle = cComGetHandle(TmpFileName);
            }

            if (-1 != FileHandle)
            {
				// TODO: some files shite
				//pTxBuf->pFile = (FIL*)&(GH.ComInstance.Files[FileHandle]);
				//pTxBuf->pFile->File = open(pTxBuf->pFile->Name, O_RDONLY, 0x444);

				//// Get file length
				//pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
				//lseek(pTxBuf->pFile->File, 0L, SEEK_SET);

				//pDlMsg->CmdSize = 0x00;                               // Msg Len
				//pDlMsg->MsgCount = 0x00;                               // Msg Count
				//pDlMsg->CmdType = SYSTEM_COMMAND_REPLY;               // Cmd Type
				//pDlMsg->Cmd = BEGIN_DOWNLOAD;                     // Cmd

				//pDlMsg->FileSizeLsb = (UBYTE)(pTxBuf->pFile->Size);       // File Size Lsb
				//pDlMsg->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8); // File Size
				//pDlMsg->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16); // File Size
				//pDlMsg->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24); // File Size Msb

				//Index = (ushort)(CommonHelper.strlen((DATA8*)pTxBuf->pFile->Name) + 1);
				//CommonHelper.snprintf((DATA8*)(&(pTxBuf->Buf[SIZEOF_BEGINDL])), Index, CommonHelper.GetString((DATA8*)pTxBuf->pFile->Name));

				//Index += SIZEOF_BEGINDL;

				//// Find the number of bytes to go into this message
				//if ((MAX_MSG_SIZE - Index) < pTxBuf->pFile->Size)
				//{
				//    // Msg cannot hold complete file
				//    pTxBuf->Buf[0] = (UBYTE)((MAX_MSG_SIZE - 2) & 0xff);
				//    pTxBuf->Buf[1] = (UBYTE)(((MAX_MSG_SIZE - 2) >> 8) & 0xff);

				//    ReadBytes = read(pTxBuf->pFile->File, &(pTxBuf->Buf[Index]), (pTxBuf->BufSize - Index));
				//    pTxBuf->BlockLen = pTxBuf->BufSize;

				//    pTxBuf->SubState = FILE_IN_PROGRESS_WAIT_FOR_REPLY;
				//    pTxBuf->SendBytes = ReadBytes;                       // No of bytes send in message
				//    pTxBuf->pFile->Pointer = ReadBytes;                       // No of bytes read from file
				//    pTxBuf->MsgLen = (uint)((pDlMsg->CmdSize - Index) + 2);   // Index = full header size
				//    pTxBuf->Writing = 1;
				//}
				//else
				//{
				//    // Msg can hold complete file
				//    pTxBuf->Buf[0] = (UBYTE)(pTxBuf->pFile->Size + Index - 2);
				//    pTxBuf->Buf[1] = (UBYTE)((pTxBuf->pFile->Size + Index - 2) >> 8);

				//    if ((pTxBuf->BufSize - Index) < pTxBuf->pFile->Size)
				//    {
				//        // Complete msg exceeds buffer size
				//        ReadBytes = read(pTxBuf->pFile->File, &(pTxBuf->Buf[Index]), (pTxBuf->BufSize - Index));
				//        pTxBuf->BlockLen = pTxBuf->BufSize;
				//    }
				//    else
				//    {
				//        // Complete msg can fit in buffer
				//        ReadBytes = read(pTxBuf->pFile->File, &(pTxBuf->Buf[Index]), pTxBuf->pFile->Size);
				//        pTxBuf->BlockLen = pTxBuf->pFile->Size + Index;

				//        // Close handles
				//        cComCloseFileHandle(&(pTxBuf->pFile->File));
				//        cComFreeHandle(FileHandle);
				//    }

				//    pTxBuf->SubState = FILE_COMPLETE_WAIT_FOR_REPLY;
				//    pTxBuf->SendBytes = ReadBytes;                       // No of bytes send in message
				//    pTxBuf->pFile->Pointer = ReadBytes;                       // No of bytes read from file
				//    pTxBuf->MsgLen = (uint)((pDlMsg->CmdSize - Index) + 2);   // Index = full header size
				//    pTxBuf->Writing = 1;
				//}
				GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
			}
        }

        public void cComCreateContinueDl(RXBUF* pRxBuf, TXBUF* pTxBuf)
        {
            UWORD ReadBytes;
            CONTINUE_DL* pContinueDl;
            RPLY_BEGIN_DL* pRplyBeginDl;

            pRplyBeginDl = (RPLY_BEGIN_DL*)&(pRxBuf->Buf[0]);
            pContinueDl = (CONTINUE_DL*)&(pTxBuf->Buf[0]);

            pContinueDl->CmdSize = SIZEOF_CONTINUEDL - sizeof(CMDSIZE);
            pContinueDl->MsgCount = (pRplyBeginDl->MsgCount)++;
            pContinueDl->CmdType = SYSTEM_COMMAND_REPLY;
            pContinueDl->Cmd = CONTINUE_DOWNLOAD;
            pContinueDl->Handle = pTxBuf->RemoteFileHandle;

            if ((MAX_MSG_SIZE - SIZEOF_CONTINUEDL) < (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer))
            {
                // Msg cannot hold complete file
                using var fs = File.OpenRead(CommonHelper.GetString(pTxBuf->pFile->Name));
                ReadBytes = (ushort)fs.ReadUnsafe(&(pTxBuf->Buf[SIZEOF_CONTINUEDL]), 0, (int)(pTxBuf->BufSize - SIZEOF_CONTINUEDL));
                fs.Close();
                pTxBuf->BlockLen = pTxBuf->BufSize;

                pContinueDl->CmdSize += ReadBytes;
                pTxBuf->SubState = FILE_IN_PROGRESS_WAIT_FOR_REPLY;
                pTxBuf->SendBytes = ReadBytes;                                       // No of bytes send in message
                pTxBuf->pFile->Pointer += ReadBytes;                                       // No of bytes read from file
                pTxBuf->MsgLen = (uint)((pContinueDl->CmdSize - SIZEOF_CONTINUEDL) + 2);  // Index = full header size
                pTxBuf->Writing = 1;
            }
            else
            {
				using var fs = File.OpenRead(CommonHelper.GetString(pTxBuf->pFile->Name));
				// Msg can hold complete file
				if ((pTxBuf->BufSize - SIZEOF_CONTINUEDL) < (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer))
                {
                    // Complete msg exceeds buffer size
                    ReadBytes = (ushort)fs.ReadUnsafe(&(pTxBuf->Buf[SIZEOF_CONTINUEDL]), 0, (int)(pTxBuf->BufSize - SIZEOF_CONTINUEDL));
                }
                else
                {
                    // Complete msg can fit in buffer
                    ReadBytes = (ushort)fs.ReadUnsafe(&(pTxBuf->Buf[SIZEOF_CONTINUEDL]), 0, (int)(pTxBuf->pFile->Size - pTxBuf->pFile->Pointer));

                    // Close handles
                    cComCloseFileHandle(&(pTxBuf->pFile->File));
                    cComFreeHandle((sbyte)pTxBuf->FileHandle);
                }
				fs.Close();

				pTxBuf->BlockLen = (uint)(ReadBytes + SIZEOF_CONTINUEDL);
                pContinueDl->CmdSize += ReadBytes;
                pTxBuf->SubState = FILE_COMPLETE_WAIT_FOR_REPLY;
                pTxBuf->SendBytes = ReadBytes;                       // No of bytes send in message
                pTxBuf->pFile->Pointer += ReadBytes;                       // No of bytes read from file
                pTxBuf->MsgLen = (uint)((pContinueDl->CmdSize - SIZEOF_CONTINUEDL) + 2);   // Index = full header size
                pTxBuf->Writing = 1;
            }
        }


        public void cComSystemReply(RXBUF* pRxBuf, TXBUF* pTxBuf)
        {
            COMCMD* pComCmd;
            SYSCMDC* pSysCmdC;
            CMDSIZE CmdSize;
            UBYTE* FileName = CommonHelper.Pointer1d<UBYTE>(MAX_FILENAME_SIZE);

            pComCmd = (COMCMD*)pRxBuf->Buf;
            pSysCmdC = (SYSCMDC*)(*pComCmd).PayLoad;
            CmdSize = (*pComCmd).CmdSize;

            switch ((*pSysCmdC).Sys)
            {
                case BEGIN_DOWNLOAD:
                    {
                        // This is the reply from the begin download command
                        // This is part of the file transfer sequence
                        // Check for single file or Folder transfer

                        RPLY_BEGIN_DL* pRplyBeginDl;

                        pRplyBeginDl = (RPLY_BEGIN_DL*)&(pRxBuf->Buf[0]);

                        if ((SUCCESS == pRplyBeginDl->Status) || (END_OF_FILE == pRplyBeginDl->Status))
                        {
                            pTxBuf->RemoteFileHandle = pRplyBeginDl->Handle;

                            if (FILE_IN_PROGRESS_WAIT_FOR_REPLY == pTxBuf->SubState)
                            {
                                // Issue continue write as file is not
                                // completely downloaded
                                cComCreateContinueDl(pRxBuf, pTxBuf);
                            }
                            else
                            {
                                if (FILE_COMPLETE_WAIT_FOR_REPLY == pTxBuf->SubState)
                                {
                                    // Complete file downloaded check for more files
                                    // to download
                                    if (TXFOLDER == pTxBuf->State)
                                    {
                                        if (cComGetNxtFile(pTxBuf->pDir, FileName) != 0)
                                        {
                                            cComCreateBeginDl(pTxBuf, FileName);
                                        }
                                        else
                                        {
                                            // No More files
                                            pTxBuf->State = TXIDLE;
                                            pTxBuf->SubState = SUBSTATE_IDLE;
                                            GH.ComInstance.ComResult = OK;
                                        }
                                    }
                                    else
                                    {
                                        // Only one file to send
                                        pTxBuf->State = TXIDLE;
                                        pTxBuf->SubState = SUBSTATE_IDLE;
                                        GH.ComInstance.ComResult = OK;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case CONTINUE_DOWNLOAD:
                    {

                        RPLY_CONTINUE_DL* pRplyContinueDl;
                        pRplyContinueDl = (RPLY_CONTINUE_DL*)&(pRxBuf->Buf[0]);

                        if ((SUCCESS == pRplyContinueDl->Status) || (END_OF_FILE == pRplyContinueDl->Status))
                        {
                            if (FILE_IN_PROGRESS_WAIT_FOR_REPLY == pTxBuf->SubState)
                            {
                                // Issue continue write as file is not
                                // completely downloaded
                                cComCreateContinueDl(pRxBuf, pTxBuf);
                            }
                            else
                            {
                                if (FILE_COMPLETE_WAIT_FOR_REPLY == pTxBuf->SubState)
                                {
                                    // Complete file downloaded check for more files
                                    // to download
                                    if (TXFOLDER == pTxBuf->State)
                                    {
                                        if (cComGetNxtFile(pTxBuf->pDir, FileName) != 0)
                                        {
                                            cComCreateBeginDl(pTxBuf, FileName);
                                        }
                                        else
                                        {
                                            pTxBuf->State = TXIDLE;
                                            pTxBuf->SubState = SUBSTATE_IDLE;
                                            GH.ComInstance.ComResult = OK;
                                        }
                                    }
                                    else
                                    {
                                        pTxBuf->State = TXIDLE;
                                        pTxBuf->SubState = SUBSTATE_IDLE;
                                        GH.ComInstance.ComResult = OK;
                                    }
                                }
                            }
                        }
                    }
                    break;

                default:
                    {
                    }
                    break;
            }
        }


        public void cComGetNameFromScandirList(DATA8* NameList, DATA8* pBuffer, ULONG* pNameLen, UBYTE* pFolder)
        {
            DATA8* FileName = CommonHelper.Pointer1d<DATA8>(MD5LEN + 1 + FILENAMESIZE);
            // struct stat FileInfo;
            ULONG* Md5Sum = CommonHelper.Pointer1d<ULONG>(4);

            *pNameLen = 0;

            //If type is a directory the add "/" at the end
            if (Directory.Exists(CommonHelper.GetString(NameList)))
            {
                CommonHelper.strncpy(pBuffer, NameList, FILENAMESIZE);   // Need to copy to tmp var to be able to add "/" for folders
                *pNameLen = (uint)CommonHelper.strlen(NameList);               // + 1 is the new line character

                CommonHelper.strcat(pBuffer, "/".AsSbytePointer());
                (*pNameLen)++;

                CommonHelper.strcat(pBuffer, "\n".AsSbytePointer());
                (*pNameLen)++;

            }
            else
            {

                if (File.Exists(CommonHelper.GetString(NameList)))
                {
					/* If it is a file then add 16 bytes MD5SUM + space */
					CommonHelper.strcpy(FileName, (DATA8*)pFolder);
                    CommonHelper.strcat(FileName, "/".AsSbytePointer());
                    CommonHelper.strcat(FileName, NameList);

                    /* Get the MD5sum and put in the buffer */
                    GH.Md5.md5_file(FileName, 0, (UBYTE*)Md5Sum);
                    *pNameLen = (uint)CommonHelper.sprintf(pBuffer, $"{((UBYTE*)Md5Sum)[0]:B}{((UBYTE*)Md5Sum)[1]:B}{((UBYTE*)Md5Sum)[2]:B}{((UBYTE*)Md5Sum)[3]:B}{((UBYTE*)Md5Sum)[4]:B}{((UBYTE*)Md5Sum)[5]:B}{((UBYTE*)Md5Sum)[6]:B}{((UBYTE*)Md5Sum)[7]:B}{((UBYTE*)Md5Sum)[8]:B}{((UBYTE*)Md5Sum)[9]:B}{((UBYTE*)Md5Sum)[10]:B}{((UBYTE*)Md5Sum)[11]:B}{((UBYTE*)Md5Sum)[12]:B}{((UBYTE*)Md5Sum)[13]:B}{((UBYTE*)Md5Sum)[14]:B}{((UBYTE*)Md5Sum)[15]:B} ");

                    /* Insert file size */
                    FileInfo fi = new FileInfo(CommonHelper.GetString(FileName));
                    *pNameLen += (uint)CommonHelper.sprintf(&(pBuffer[MD5LEN + 1]), $"{(ULONG)fi.Length:b8} ");

                    /* Insert Filename */
                    CommonHelper.strcat(pBuffer, NameList);
                    *pNameLen += (uint)CommonHelper.strlen(NameList);

                    CommonHelper.strcat(pBuffer, "\n".AsSbytePointer());
                    (*pNameLen)++;
                }
            }
        }


        public UBYTE cComCheckForSpace(DATA8* pFullName, ULONG Size)
        {
            UBYTE RtnVal;
            DATA32 TotalSize;
            DATA32 FreeSize;
            DATA8 Changed;

            RtnVal = (byte)RESULT.FAIL;

            //Make it KB and round up
            if ((Size & (ULONG)(KB - 1)) != 0)
            {
                Size /= KB;
                Size += 1;
            }
            else
            {
                Size /= KB;
            }

            if (CommonHelper.strstr(pFullName, SDCARD_FOLDER.AsSbytePointer()) != null)
            {
                if (GH.Lms.CheckSdcard(&Changed, &TotalSize, &FreeSize, 1) != 0)
                {
                    if (FreeSize >= Size)
                    {
                        RtnVal = OK;
                    }
                }
            }
            else
            {
                if (CommonHelper.strstr(pFullName, USBSTICK_FOLDER.AsSbytePointer()) != null)
                {
                    if (GH.Lms.CheckUsbstick(&Changed, &TotalSize, &FreeSize, 1) != 0)
                    {
                        if (FreeSize >= Size)
                        {
                            RtnVal = OK;
                        }
                    }
                }
                else
                {
                    GH.Memory.cMemoryGetUsage(&TotalSize, &FreeSize, 1);
                    if (FreeSize >= Size + 300)
                    {
                        RtnVal = OK;
                    }
                }
            }
            return (RtnVal);
        }


        /*! \page ComModule
         *
         *  <hr size="1"/>
         *  <b>     write </b>
         */
        /*! \brief    cSystemCommand
         *
         *
         *  cComSystemCommand
         *  -----------------
         *
         *
         *
         *    One message at a time principles
         *    --------------------------------
         *
         *     - The system is only able to process one command at a time (both SYS and DIR commands).
         *     - GH.ComInstance.ReplyStatus holds the information about command processing.
         *     - Writing flag indicated that the reply is ready to be transmitted
         *     - Direct commands are commands to be interpreted by VM - Direct commands are not interpreted until process is
         *       returned to the VM
         *     - System commands are usually processed when received except for large messages (messages larger that buffersize)
         *
         *    Direct command              -> When received     ->  if reply required        ->  GH.ComInstance.ReplyStatus = DIR_CMD_REPLY
         *                                         |
         *                                          ------------>  if no reply required     ->  GH.ComInstance.ReplyStatus = DIR_CMD_NOREPLY
         *
         *    VM reply to direct command  -> if (GH.ComInstance.ReplyStatus = DIR_CMD_REPLY)   ->  pTxBuf->Writing         = 1
         *    (VM always replies, after interp.)                                                GH.ComInstance.ReplyStatus = 0
         *               |
         *                -----------------> if (GH.ComInstance.ReplyStatus = DIR_CMD_NOREPLY) ->  pTxBuf->Writing         = 0
         *                                                                                      GH.ComInstance.ReplyStatus = 0
         *
         *
         *    System command              -> if reply required ->  GH.ComInstance.ReplyStatus = SYS_CMD_REPLY   -> if (pRxBuf->State  =  RXFILEDL) -> Do nothing
         *         |                                                                 |
         *         |                                                                  ------------------------> if (pRxBuf->State  != RXFILEDL) -> pTxBuf->Writing         = 1
         *         |                                                                                                                               GH.ComInstance.ReplyStatus = 0
         *         |
         *          -----------------------> If reply not req. ->  GH.ComInstance.ReplyStatus = SYS_CMD_NOREPLY -> if (pRxBuf->State  =  RXFILEDL) -> Do nothing
         *                                                                           |
         *                                                                            ------------------------> if (pRxBuf->State  != RXFILEDL) -> GH.ComInstance.ReplyStatus = 0
         *
         *
         *
         *    File Download - Large messages
         *    ------------------------------
         *
         *    CONTINUE_DOWNLOAD
         *    BEGIN_DOWNLOAD    --->   (Message size <= Buffer size)  -->  Write bytes to file
         *         |                                                  -->  pRxBuf->State  =  RXIDLE
         *         |
         *         |
         *          --------------->   (Message size  > Buffer Size)  -->  Write bytes from buffer
         *                                                            -->  pRxBuf->State  =  RXFILEDL
         *
         *          Buffer full --->   if pRxBuf->State  =  RXFILEDL  -->  Yes -> write into buffer
         *                                           |                                   |
         *                                           |                                    ------------------> if Remainig msg = 0  ->  pRxBuf->State  =  RXIDLE
         *                                           |
         *                                            ------------------>  No  ->  Interprete as new command
         *
         *
         *
         */
        public void cComSystemCommand(RXBUF* pRxBuf, TXBUF* pTxBuf)
        {
            COMCMD* pComCmd;
            SYSCMDC* pSysCmdC;
            CMDSIZE CmdSize;
            int Tmp;
            DATA8* Folder = CommonHelper.Pointer1d<DATA8>(60);
            ULONG BytesToWrite;
            DATA8 FileHandle = 0;

            pComCmd = (COMCMD*)pRxBuf->Buf;
            pSysCmdC = (SYSCMDC*)(*pComCmd).PayLoad;
            CmdSize = (*pComCmd).CmdSize;

            switch ((*pSysCmdC).Sys)
            {
                case BEGIN_DOWNLOAD:
                    {
                        BEGIN_DL* pBeginDl;
                        RPLY_BEGIN_DL* pReplyDl;
                        ULONG MsgHeaderSize;

                        //Setup pointers
                        pBeginDl = (BEGIN_DL*)pRxBuf->Buf;
                        pReplyDl = (RPLY_BEGIN_DL*)pTxBuf->Buf;

                        // Get file handle
                        FileHandle = cComGetHandle((DATA8*)pBeginDl->Path);
                        pRxBuf->pFile = &(GH.ComInstance.Files[FileHandle]);

                        // Fill the reply
                        pReplyDl->CmdSize = SIZEOF_RPLYBEGINDL - sizeof(CMDSIZE);
                        pReplyDl->MsgCount = pBeginDl->MsgCount;
                        pReplyDl->CmdType = SYSTEM_REPLY;
                        pReplyDl->Cmd = BEGIN_DOWNLOAD;
                        pReplyDl->Status = SUCCESS;
                        pReplyDl->Handle = (byte)FileHandle;

                        if (FileHandle >= 0)
                        {
                            pRxBuf->pFile->Size = (ULONG)(pBeginDl->FileSizeLsb);
                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeNsb1) << 8;
                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeNsb2) << 16;
                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeMsb) << 24;

                            if (OK == cComCheckForSpace(&(GH.ComInstance.Files[FileHandle].Name[0]), pRxBuf->pFile->Size))
                            {

                                pRxBuf->pFile->Length = (ULONG)0;
                                pRxBuf->pFile->Pointer = (ULONG)0;

                                Tmp = 0;
                                while ((GH.ComInstance.Files[FileHandle].Name[Tmp] != 0) && (Tmp < (MAX_FILENAME_SIZE - 1)))
                                {
                                    Folder[Tmp] = GH.ComInstance.Files[FileHandle].Name[Tmp];
                                    if (Folder[Tmp] == '/')
                                    {
                                        Folder[Tmp + 1] = 0;
                                        if (CommonHelper.strcmp("~/".AsSbytePointer(), Folder) != 0)
                                        {
                                            if (Directory.CreateDirectory(CommonHelper.GetString(Folder)) != null)
                                            {
                                                // chmod(Folder, S_IRWXU | S_IRWXG | S_IRWXO);
                                                GH.printf($"Folder {CommonHelper.GetString(Folder)} created\r\n");
                                            }
                                            else
                                            {
                                                GH.printf($"Folder {CommonHelper.GetString(Folder)} not created (%s)\r\n");
                                            }
                                        }
                                    }
                                    Tmp++;
                                }

                                pRxBuf->pFile->File = 1;// open(pRxBuf->pFile->Name, O_CREAT | O_WRONLY | O_TRUNC | O_SYNC, 0x666);

                                if (pRxBuf->pFile->File >= 0)
                                {
                                    MsgHeaderSize = ((uint)(CommonHelper.strlen((DATA8*)pBeginDl->Path) + 1 + SIZEOF_BEGINDL)); // +1 = zero termination
                                    pRxBuf->MsgLen = (uint)(CmdSize + sizeof(CMDSIZE) - MsgHeaderSize);

                                    if (CmdSize <= (pRxBuf->BufSize - sizeof(CMDSIZE)))
                                    {
                                        // All data included in this buffer
                                        BytesToWrite = pRxBuf->MsgLen;
                                        pRxBuf->State = RXIDLE;
                                    }
                                    else
                                    {
                                        // Rx buffer must be read more that one to receive the complete message
                                        BytesToWrite = pRxBuf->BufSize - MsgHeaderSize;
                                        pRxBuf->State = RXFILEDL;
                                    }
                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

                                    if (BytesToWrite > (GH.ComInstance.Files[FileHandle].Size))
                                    {
                                        // If Bytes to write into the file is bigger than file size -> Error message
                                        pReplyDl->Status = SIZE_ERROR;
                                        BytesToWrite = GH.ComInstance.Files[FileHandle].Size;
                                    }

                                    File.WriteAllBytes(CommonHelper.GetString(pRxBuf->pFile->Name), CommonHelper.GetArray(&(pRxBuf->Buf[MsgHeaderSize]), (int)BytesToWrite));
                                    GH.ComInstance.Files[FileHandle].Length += (ULONG)BytesToWrite;
                                    pRxBuf->RxBytes = (ULONG)BytesToWrite;
                                    pRxBuf->pFile->Pointer = (ULONG)BytesToWrite;

                                    if (pRxBuf->pFile->Pointer >= pRxBuf->pFile->Size)
                                    {
                                        cComCloseFileHandle(&(pRxBuf->pFile->File));
                                        // chmod(GH.ComInstance.Files[FileHandle].Name, S_IRWXU | S_IRWXG | S_IRWXO);
                                        cComFreeHandle(FileHandle);

                                        pReplyDl->Status = END_OF_FILE;
                                        pRxBuf->State = RXIDLE;
                                    }
                                }
                                else
                                {
                                    // Error in opening file
                                    GH.printf($"File {CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} not created\r\n");
                                    cComFreeHandle(FileHandle);
                                    pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
                                    pReplyDl->Status = UNKNOWN_HANDLE;
                                    pReplyDl->Handle = byte.MaxValue;
                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

                                }
                            }
                            else
                            {
                                //Not enough space for the file
                                GH.printf($"File {CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} is too big\r\n");

                                cComFreeHandle(FileHandle);
                                pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
                                pReplyDl->Status = SIZE_ERROR;
                                pReplyDl->Handle = byte.MaxValue;
								pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;
                            }
                        }
                        else
                        {
                            // Error in getting a handle
                            pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyDl->Status = NO_HANDLES_AVAILABLE;
                            pReplyDl->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

                            GH.printf("No more handles\r\n");
                        }
                    }
                    break;

                case CONTINUE_DOWNLOAD:
                    {
                        CONTINUE_DL* pContiDl;
                        RPLY_CONTINUE_DL* pRplyContiDl;
                        ULONG BytesToWriteLocal;

                        //Setup pointers
                        pContiDl = (CONTINUE_DL*)pComCmd;
                        pRplyContiDl = (RPLY_CONTINUE_DL*)pTxBuf->Buf;

                        // Get handle
                        FileHandle = (sbyte)pContiDl->Handle;

                        // Fill the reply
                        pRplyContiDl->CmdSize = SIZEOF_RPLYCONTINUEDL - sizeof(CMDSIZE);
                        pRplyContiDl->MsgCount = pContiDl->MsgCount;
                        pRplyContiDl->CmdType = SYSTEM_REPLY;
                        pRplyContiDl->Cmd = CONTINUE_DOWNLOAD;
                        pRplyContiDl->Status = SUCCESS;
                        pRplyContiDl->Handle = (byte)FileHandle;

                        if ((FileHandle >= 0) && (GH.ComInstance.Files[FileHandle].Name[0] != 0))
                        {

                            pRxBuf->FileHandle = (byte)FileHandle;
                            pRxBuf->pFile = &(GH.ComInstance.Files[FileHandle]);

                            if (pRxBuf->pFile->File >= 0)
                            {

                                pRxBuf->MsgLen = (uint)((pContiDl->CmdSize + sizeof(CMDSIZE)) - SIZEOF_CONTINUEDL);
                                pRxBuf->RxBytes = 0;

                                if (pContiDl->CmdSize <= (pRxBuf->BufSize - sizeof(CMDSIZE)))
                                {
									// All data included in this buffer
									BytesToWriteLocal = pRxBuf->MsgLen;
                                    pRxBuf->State = RXIDLE;
                                }
                                else
                                {
									// Rx buffer must be read more that one to receive the complete message
									BytesToWriteLocal = pRxBuf->BufSize - SIZEOF_CONTINUEDL;
                                    pRxBuf->State = RXFILEDL;
                                }
                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;

                                if ((pRxBuf->pFile->Pointer + pRxBuf->MsgLen) > pRxBuf->pFile->Size)
                                {
									BytesToWriteLocal = pRxBuf->pFile->Size - pRxBuf->pFile->Pointer;

                                    GH.printf("File size limited\r\n");
                                }

                                File.WriteAllBytes(CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name), CommonHelper.GetArray((pContiDl->PayLoad), (int)BytesToWriteLocal));
                                pRxBuf->pFile->Pointer += BytesToWriteLocal;
                                pRxBuf->RxBytes = BytesToWriteLocal;

                                GH.printf($"Size {(ulong)GH.ComInstance.Files[FileHandle].Size} - Loaded {(ulong)GH.ComInstance.Files[FileHandle].Length}\r\n");

                                if (pRxBuf->pFile->Pointer >= GH.ComInstance.Files[FileHandle].Size)
                                {
                                    GH.printf($"{CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} {(ulong)GH.ComInstance.Files[FileHandle].Length} bytes downloaded\r\n");

                                    cComCloseFileHandle(&(GH.ComInstance.Files[FileHandle].File));
                                    // chmod(GH.ComInstance.Files[FileHandle].Name, S_IRWXU | S_IRWXG | S_IRWXO);
                                    cComFreeHandle(FileHandle);
                                    pRplyContiDl->Status = END_OF_FILE;
                                }
                            }
                            else
                            {
                                // Illegal file
                                pRplyContiDl->CmdType = SYSTEM_REPLY_ERROR;
                                pRplyContiDl->Status = UNKNOWN_ERROR;
                                pRplyContiDl->Handle = byte.MaxValue;
								pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;
                                cComFreeHandle(FileHandle);

                                GH.printf("Data not appended\r\n");
                            }
                        }
                        else
                        {
                            // Illegal file handle
                            pRplyContiDl->CmdType = SYSTEM_REPLY_ERROR;
                            pRplyContiDl->Status = UNKNOWN_HANDLE;
                            pRplyContiDl->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;

                            GH.printf("Invalid handle\r\n");
                        }
                    }
                    break;

                case BEGIN_UPLOAD:
                    {
                        ULONG BytesToRead;
                        ULONG ReadBytes;

                        BEGIN_READ* pBeginRead; ;
                        RPLY_BEGIN_READ* pReplyBeginRead;

                        //Setup pointers
                        pBeginRead = (BEGIN_READ*)pRxBuf->Buf;
                        pReplyBeginRead = (RPLY_BEGIN_READ*)pTxBuf->Buf;

                        FileHandle = cComGetHandle((DATA8*)pBeginRead->Path);

                        pTxBuf->pFile = &GH.ComInstance.Files[FileHandle];  // Insert the file pointer into the ch struct
                        pTxBuf->FileHandle = (byte)FileHandle;                      // Also save the File handle number

                        pReplyBeginRead->CmdSize = SIZEOF_RPLYBEGINREAD - sizeof(CMDSIZE);
                        pReplyBeginRead->MsgCount = pBeginRead->MsgCount;
                        pReplyBeginRead->CmdType = SYSTEM_REPLY;
                        pReplyBeginRead->Cmd = BEGIN_UPLOAD;
                        pReplyBeginRead->Status = SUCCESS;
                        pReplyBeginRead->Handle = (byte)FileHandle;

                        if (FileHandle >= 0)
                        {
                            BytesToRead = (ULONG)(pBeginRead->BytesToReadLsb);
                            BytesToRead += (ULONG)(pBeginRead->BytesToReadMsb) << 8;

							// TODO: some files shite
							//pTxBuf->pFile->File = open(pTxBuf->pFile->Name, O_RDONLY, 0x444);

							//if (pTxBuf->pFile->File >= 0)
							//{
							//    // Get file length
							//    pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
							//    lseek(pTxBuf->pFile->File, 0L, SEEK_SET);

							//    pTxBuf->MsgLen = BytesToRead;

							//    if (BytesToRead > pTxBuf->pFile->Size)
							//    {
							//        // if message length  > filesize then crop message
							//        pTxBuf->MsgLen = pTxBuf->pFile->Size;
							//    }

							//    if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINREAD) <= pTxBuf->BufSize)
							//    {
							//        // Read all requested bytes as they can fit into the buffer
							//        ReadBytes = read(pTxBuf->pFile->File, pReplyBeginRead->PayLoad, pTxBuf->MsgLen);
							//        pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYBEGINREAD;
							//        pTxBuf->State = TXIDLE;
							//    }
							//    else
							//    {
							//        // Read only up to full buffer size
							//        ReadBytes = read(pTxBuf->pFile->File, pReplyBeginRead->PayLoad, (pTxBuf->BufSize - SIZEOF_RPLYBEGINREAD));
							//        pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYBEGINREAD;
							//        pTxBuf->State = TXFILEUPLOAD;
							//    }

							//    pTxBuf->SendBytes = ReadBytes;  // No of bytes send in message
							//    pTxBuf->pFile->Pointer = ReadBytes;  // No of bytes read from file

							//    GH.printf($"Bytes to read {BytesToRead} bytes read {ReadBytes}\r\n");
							//    GH.printf($"FileSize: {pTxBuf->pFile->Size} \r\n");

							//    pReplyBeginRead->CmdSize += (CMDSIZE)pTxBuf->MsgLen;
							//    pReplyBeginRead->FileSizeLsb = (UBYTE)(pTxBuf->pFile->Size);
							//    pReplyBeginRead->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
							//    pReplyBeginRead->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
							//    pReplyBeginRead->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

							//    if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
							//    {
							//        GH.printf($"{CommonHelper.GetString(pTxBuf->pFile->Name)} {(ulong)pTxBuf->pFile->Length} bytes UpLoaded\r\n");

							//        pReplyBeginRead->Status = END_OF_FILE;
							//        cComCloseFileHandle(&(pTxBuf->pFile->File));
							//        cComFreeHandle(FileHandle);
							//    }
							//    cComPrintTxMsg(pTxBuf);
							//}
							//else
							//{
							//    pReplyBeginRead->CmdType = SYSTEM_REPLY_ERROR;
							//    pReplyBeginRead->Status = UNKNOWN_HANDLE;
							//    pReplyBeginRead->Handle = byte.MaxValue;
							//    pTxBuf->BlockLen = SIZEOF_RPLYBEGINREAD;
							//    cComFreeHandle(FileHandle);

							//    GH.printf($"File {CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} is not present \r\n");
							//}

							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
						}
                        else
                        {
                            pReplyBeginRead->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyBeginRead->Status = NO_HANDLES_AVAILABLE;
                            pReplyBeginRead->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYBEGINREAD;

                            GH.printf("No more handles\r\n");
                        }

                    }
                    break;

                case CONTINUE_UPLOAD:
                    {
                        ULONG BytesToRead;
                        ULONG ReadBytes = 0;

                        CONTINUE_READ* pContinueRead;
                        RPLY_CONTINUE_READ* pReplyContinueRead;

                        //Setup pointers
                        pContinueRead = (CONTINUE_READ*)pRxBuf->Buf;
                        pReplyContinueRead = (RPLY_CONTINUE_READ*)pTxBuf->Buf;

                        FileHandle = (sbyte)pContinueRead->Handle;

                        /* Fill out the default settings */
                        pReplyContinueRead->CmdSize = SIZEOF_RPLYCONTINUEREAD - sizeof(CMDSIZE);
                        pReplyContinueRead->MsgCount = pContinueRead->MsgCount;
                        pReplyContinueRead->CmdType = SYSTEM_REPLY;
                        pReplyContinueRead->Cmd = CONTINUE_UPLOAD;
                        pReplyContinueRead->Status = SUCCESS;
                        pReplyContinueRead->Handle = (byte)FileHandle;

                        if ((FileHandle >= 0) && (pTxBuf->pFile->Name[0] != 0))
                        {

                            if (pTxBuf->pFile->File >= 0)
                            {
                                BytesToRead = (ULONG)(pContinueRead->BytesToReadLsb);
                                BytesToRead += (ULONG)(pContinueRead->BytesToReadMsb) << 8;

                                // If host is asking for more bytes than remaining file size then
                                // message length needs to adjusted accordingly
                                if ((pTxBuf->pFile->Size - pTxBuf->pFile->Pointer) > BytesToRead)
                                {
                                    pTxBuf->MsgLen = BytesToRead;
                                }
                                else
                                {
                                    pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
                                    BytesToRead = pTxBuf->MsgLen;
                                }

								// TODO: some files shite
								//if ((BytesToRead + SIZEOF_RPLYCONTINUEREAD) <= pTxBuf->BufSize)
								//{
								//    // Read all requested bytes as they can fit into the buffer
								//    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueRead->PayLoad, (size_t)BytesToRead);
								//    pTxBuf->BlockLen = BytesToRead + SIZEOF_RPLYCONTINUEREAD;
								//    pTxBuf->State = TXIDLE;
								//}
								//else
								//{
								//    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueRead->PayLoad, (size_t)(pTxBuf->BufSize - SIZEOF_RPLYCONTINUEREAD));
								//    pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYCONTINUEREAD;
								//    pTxBuf->State = TXFILEUPLOAD;
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

								pTxBuf->SendBytes = (ULONG)ReadBytes;
                                pTxBuf->pFile->Pointer += (ULONG)ReadBytes;
                                pReplyContinueRead->CmdSize += (ushort)pTxBuf->MsgLen;

                                cComPrintTxMsg(pTxBuf);

                                GH.printf($"Size {(ulong)pTxBuf->pFile->Size} - Loaded {(ulong)pTxBuf->pFile->Length}\r\n");

                                if (GH.ComInstance.Files[FileHandle].Pointer >= GH.ComInstance.Files[FileHandle].Size)
                                {
                                    GH.printf($"{CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} {(ulong)pTxBuf->pFile->Length} bytes UpLoaded\r\n");

                                    pReplyContinueRead->Status = END_OF_FILE;
                                    cComCloseFileHandle(&(pTxBuf->pFile->File));
                                    cComFreeHandle(FileHandle);
                                }
                            }
                            else
                            {
                                pReplyContinueRead->CmdType = SYSTEM_REPLY_ERROR;
                                pReplyContinueRead->Status = HANDLE_NOT_READY;
                                pReplyContinueRead->Handle = byte.MaxValue;
                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEREAD;
                                cComFreeHandle(FileHandle);
                                GH.printf("Data not read\r\n");
                            }
                        }
                        else
                        {
                            pReplyContinueRead->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyContinueRead->Status = UNKNOWN_HANDLE;
                            pReplyContinueRead->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEREAD;
                            GH.printf("Invalid handle\r\n");
                        }
                    }
                    break;

                case BEGIN_GETFILE:
                    {

                        ULONG BytesToRead;
                        ULONG ReadBytes;
                        BEGIN_GET_FILE* pBeginGetFile; ;
                        RPLY_BEGIN_GET_FILE* pReplyBeginGetFile;

                        //Setup pointers
                        pBeginGetFile = (BEGIN_GET_FILE*)pRxBuf->Buf;
                        pReplyBeginGetFile = (RPLY_BEGIN_GET_FILE*)pTxBuf->Buf;

                        FileHandle = cComGetHandle((DATA8*)pBeginGetFile->Path);
                        pTxBuf->pFile = &GH.ComInstance.Files[FileHandle];  // Insert the file pointer into the ch struct
                        pTxBuf->FileHandle = (byte)FileHandle;                      // Also save the File handle number

                        // Fill out the reply
                        pReplyBeginGetFile->CmdSize = SIZEOF_RPLYBEGINGETFILE - sizeof(CMDSIZE);
                        pReplyBeginGetFile->MsgCount = pBeginGetFile->MsgCount;
                        pReplyBeginGetFile->CmdType = SYSTEM_REPLY;
                        pReplyBeginGetFile->Cmd = BEGIN_GETFILE;
                        pReplyBeginGetFile->Handle = (byte)FileHandle;
                        pReplyBeginGetFile->Status = SUCCESS;

                        if (FileHandle >= 0)
                        {
                            /* How many bytes to be returned the in the reply for BEGIN_UPLOAD    */
                            /* Should actually only be 2 bytes as only we can hold into 2 length  */
                            /* bytes in the protocol                                              */
                            BytesToRead = (ULONG)(pBeginGetFile->BytesToReadLsb);
                            BytesToRead += (ULONG)(pBeginGetFile->BytesToReadMsb) << 8;

                            GH.printf($"File to get:  {CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} \r\n");

							// TODO: some files shite
							//pTxBuf->pFile->File = open(pTxBuf->pFile->Name, O_RDONLY, 0x444);
							//if (pTxBuf->pFile->File >= 0)
							//{
							//    // Get file length
							//    pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
							//    lseek(pTxBuf->pFile->File, 0L, SEEK_SET);

							//    pTxBuf->MsgLen = BytesToRead;
							//    if (BytesToRead > pTxBuf->pFile->Size)
							//    {
							//        // if message length  > filesize then crop message
							//        pTxBuf->MsgLen = pTxBuf->pFile->Size;
							//    }

							//    if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINGETFILE) <= pTxBuf->BufSize)
							//    {
							//        // Read all requested bytes as they can fit into the buffer
							//        ReadBytes = read(pTxBuf->pFile->File, pReplyBeginGetFile->PayLoad, pTxBuf->MsgLen);
							//        pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYBEGINGETFILE;
							//        pTxBuf->State = TXIDLE;
							//    }
							//    else
							//    {
							//        // Read only up to full buffer size
							//        ReadBytes = read(pTxBuf->pFile->File, pReplyBeginGetFile->PayLoad, (pTxBuf->BufSize - SIZEOF_RPLYBEGINGETFILE));
							//        pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYBEGINGETFILE;
							//        pTxBuf->State = TXGETFILE;
							//    }

							//    pTxBuf->SendBytes = ReadBytes;  // No of bytes send in message
							//    pTxBuf->pFile->Pointer = ReadBytes;  // No of bytes read from file

							//    GH.printf($"Bytes to read {BytesToRead} vs. bytes read {ReadBytes}\r\n");
							//    GH.printf($"FileSize: {pTxBuf->pFile->Size} \r\n");

							//    pReplyBeginGetFile->CmdSize += (ushort)pTxBuf->MsgLen;
							//    pReplyBeginGetFile->FileSizeLsb = (UBYTE)pTxBuf->pFile->Size;
							//    pReplyBeginGetFile->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
							//    pReplyBeginGetFile->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
							//    pReplyBeginGetFile->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

							//    if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
							//    {
							//        GH.printf($"{CommonHelper.GetString(pTxBuf->pFile->Name)} {(ulong)pTxBuf->pFile->Length} bytes UpLoaded\r\n");

							//        // If last bytes has been returned and file is not open for writing
							//        if (RESULT.FAIL == GH.Memory.cMemoryCheckOpenWrite(pTxBuf->pFile->Name))
							//        {
							//            pReplyBeginGetFile->Status = END_OF_FILE;
							//            cComCloseFileHandle(&(pTxBuf->pFile->File));
							//            cComFreeHandle(FileHandle);
							//        }
							//    }

							//    cComPrintTxMsg(pTxBuf);
							//}
							//else
							//{
							//    pReplyBeginGetFile->CmdType = SYSTEM_REPLY_ERROR;
							//    pReplyBeginGetFile->Status = HANDLE_NOT_READY;
							//    pReplyBeginGetFile->Handle = byte.MaxValue;
							//    pTxBuf->BlockLen = SIZEOF_RPLYBEGINGETFILE;
							//    cComFreeHandle(FileHandle);

							//    GH.printf($"File {CommonHelper.GetString(GH.ComInstance.Files[FileHandle].Name)} is not present \r\n");
							//}

							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
						}
                        else
                        {
                            pReplyBeginGetFile->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyBeginGetFile->Status = UNKNOWN_HANDLE;
                            pReplyBeginGetFile->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYBEGINGETFILE;

                            GH.printf("No more handles\r\n");
                        }
                    }
                    break;

                case CONTINUE_GETFILE:
                    {
                        ULONG BytesToRead;
                        ULONG ReadBytes = 0;
                        CONTINUE_GET_FILE* pContinueGetFile;
                        RPLY_CONTINUE_GET_FILE* pReplyContinueGetFile;

                        //Setup pointers
                        pContinueGetFile = (CONTINUE_GET_FILE*)pRxBuf->Buf;
                        pReplyContinueGetFile = (RPLY_CONTINUE_GET_FILE*)pTxBuf->Buf;

                        FileHandle = (sbyte)pContinueGetFile->Handle;

                        /* Fill out the default settings */
                        pReplyContinueGetFile->CmdSize = SIZEOF_RPLYCONTINUEGETFILE - sizeof(CMDSIZE);
                        pReplyContinueGetFile->MsgCount = pContinueGetFile->MsgCount;
                        pReplyContinueGetFile->CmdType = SYSTEM_REPLY;
                        pReplyContinueGetFile->Cmd = CONTINUE_GETFILE;
                        pReplyContinueGetFile->Status = SUCCESS;
                        pReplyContinueGetFile->Handle = (byte)FileHandle;

                        if ((FileHandle >= 0) && (pTxBuf->pFile->Name[0] != 0))
                        {

                            if (pTxBuf->pFile->File >= 0)
                            {

                                BytesToRead = (ULONG)(pContinueGetFile->BytesToReadLsb);
                                BytesToRead += (ULONG)(pContinueGetFile->BytesToReadMsb) << 8;

								// TODO: some files shite
								//// Get new file length: Set pointer to 0 -> find end -> set where to read from
								//lseek(pTxBuf->pFile->File, 0L, SEEK_SET);
								//pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
								//lseek(pTxBuf->pFile->File, pTxBuf->pFile->Pointer, SEEK_SET);

								//// If host is asking for more bytes than remaining file size then
								//// message length needs to adjusted accordingly
								//if ((pTxBuf->pFile->Size - pTxBuf->pFile->Pointer) > BytesToRead)
								//{
								//    pTxBuf->MsgLen = BytesToRead;
								//}
								//else
								//{
								//    pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
								//    BytesToRead = pTxBuf->MsgLen;
								//}

								//if ((BytesToRead + SIZEOF_RPLYCONTINUEGETFILE) <= pTxBuf->BufSize)
								//{
								//    // Read all requested bytes as they can fit into the buffer
								//    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueGetFile->PayLoad, (size_t)BytesToRead);
								//    pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYCONTINUEGETFILE;
								//    pTxBuf->State = TXIDLE;
								//}
								//else
								//{
								//    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueGetFile->PayLoad, (size_t)(pTxBuf->BufSize - SIZEOF_RPLYCONTINUEGETFILE));
								//    pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYCONTINUEGETFILE;
								//    pTxBuf->State = TXGETFILE;
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

								pTxBuf->SendBytes = (ULONG)ReadBytes;
                                pTxBuf->pFile->Pointer += (ULONG)ReadBytes;

                                pReplyContinueGetFile->CmdSize += (ushort)pTxBuf->MsgLen;
                                pReplyContinueGetFile->FileSizeLsb = (UBYTE)(pTxBuf->pFile->Size);
                                pReplyContinueGetFile->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
                                pReplyContinueGetFile->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
                                pReplyContinueGetFile->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
                                {
                                    GH.printf($"{CommonHelper.GetString(pTxBuf->pFile->Name)} {(ulong)pTxBuf->pFile->Length} bytes UpLoaded\r\n");

                                    // If last bytes has been returned and file is not open for writing
                                    if (RESULT.FAIL == GH.Memory.cMemoryCheckOpenWrite(pTxBuf->pFile->Name))
                                    {
                                        pReplyContinueGetFile->Status = END_OF_FILE;
                                        cComCloseFileHandle(&(pTxBuf->pFile->File));
                                        cComFreeHandle(FileHandle);
                                    }
                                }

                                GH.printf($"Size {(ulong)pTxBuf->pFile->Size} - Loaded {(ulong)pTxBuf->pFile->Length}\r\n");

                                cComPrintTxMsg(pTxBuf);
                            }
                            else
                            {
                                pReplyContinueGetFile->CmdType = SYSTEM_REPLY_ERROR;
                                pReplyContinueGetFile->Status = HANDLE_NOT_READY;
                                pReplyContinueGetFile->Handle = byte.MaxValue;
                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEGETFILE;
                                cComFreeHandle(FileHandle);

                                GH.printf("Data not read\r\n");
                            }
                        }
                        else
                        {
                            pReplyContinueGetFile->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyContinueGetFile->Status = UNKNOWN_HANDLE;
                            pReplyContinueGetFile->Handle = byte.MaxValue;
							pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEGETFILE;

                            GH.printf("Invalid handle\r\n");
                        }
                    }
                    break;

                case LIST_FILES:
                    {
                        ULONG BytesToRead;
                        ULONG Len;
                        DATA8* TmpFileName = CommonHelper.Pointer1d<DATA8>(MD5LEN + 1 + SIZEOFFILESIZE + 1 + FILENAMESIZE);
                        BEGIN_LIST* pBeginList;
                        RPLY_BEGIN_LIST* pReplyBeginList;

                        //Setup pointers
                        pBeginList = (BEGIN_LIST*)pRxBuf->Buf;
                        pReplyBeginList = (RPLY_BEGIN_LIST*)pTxBuf->Buf;

                        FileHandle = cComGetHandle("ListFileHandle".AsSbytePointer());
                        pTxBuf->pFile = &GH.ComInstance.Files[FileHandle];   // Insert the file pointer into the ch struct
                        pTxBuf->FileHandle = (byte)FileHandle;                       // Also save the File handle number

                        pReplyBeginList->CmdSize = SIZEOF_RPLYBEGINLIST - sizeof(CMDSIZE);
                        pReplyBeginList->MsgCount = pBeginList->MsgCount;
                        pReplyBeginList->CmdType = SYSTEM_REPLY;
                        pReplyBeginList->Cmd = LIST_FILES;
                        pReplyBeginList->Status = SUCCESS;
                        pReplyBeginList->Handle = (byte)FileHandle;
                        pTxBuf->MsgLen = 0;

                        if (0 <= FileHandle)
                        {
                            BytesToRead = (ULONG)pBeginList->BytesToReadLsb;
                            BytesToRead += (ULONG)pBeginList->BytesToReadMsb << 8;

                            CommonHelper.snprintf((DATA8*)pTxBuf->Folder, FILENAMESIZE, CommonHelper.GetString((DATA8*)pBeginList->Path));
                            DirectoryInfo di = new DirectoryInfo(CommonHelper.GetString((DATA8*)pTxBuf->Folder));
                            var files = di.GetFiles("*", SearchOption.TopDirectoryOnly).Length;
                            pTxBuf->pFile->File = di.GetFiles("*", SearchOption.TopDirectoryOnly).Length;

							if (pTxBuf->pFile->File <= 0)
                            {
                                if (pTxBuf->pFile->File == 0)
                                {
                                    // here if no files found, equal to error as "./" and "../" normally
                                    // always found
                                    pReplyBeginList->ListSizeLsb = 0;
                                    pReplyBeginList->ListSizeNsb1 = 0;
                                    pReplyBeginList->ListSizeNsb2 = 0;
                                    pReplyBeginList->ListSizeMsb = 0;
                                    pReplyBeginList->Handle = byte.MaxValue;

                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
                                    cComFreeHandle(FileHandle);
                                }
                                else
                                {
                                    // If error occurs
                                    pReplyBeginList->CmdType = SYSTEM_REPLY_ERROR;
                                    pReplyBeginList->Status = UNKNOWN_ERROR;
                                    pReplyBeginList->ListSizeLsb = 0;
                                    pReplyBeginList->ListSizeNsb1 = 0;
                                    pReplyBeginList->ListSizeNsb2 = 0;
                                    pReplyBeginList->ListSizeMsb = 0;
                                    pReplyBeginList->Handle = byte.MaxValue;

									pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
                                    cComFreeHandle(FileHandle);
                                }
                            }
                            else
                            {
                                SLONG TmpN;
                                ULONG NameLen;
                                ULONG BytesToSend;
                                UBYTE Repeat;
                                UBYTE* pDstAdr;
                                UBYTE* pSrcAdr;

                                TmpN = pTxBuf->pFile->File;     // Make a copy of number of entries
                                Len = 0;

                                // TODO: some shite with files

                                //// Start by calculating the length of the entire list
                                //while (TmpN-- != 0)
                                //{
                                //    if ((DT_REG == pTxBuf->pFile->namelist[TmpN]->d_type) || (DT_DIR == pTxBuf->pFile->namelist[TmpN]->d_type) || (DT_LNK == pTxBuf->pFile->namelist[TmpN]->d_type))
                                //    {
                                //        Len += CommonHelper.strlen(pTxBuf->pFile->namelist[TmpN]->d_name);
                                //        if (DT_REG != pTxBuf->pFile->namelist[TmpN]->d_type)
                                //        {
                                //            // Make room room for ending "/"
                                //            Len++;
                                //        }
                                //        else
                                //        {
                                //            // Make room for md5sum + space + Filelength (fixed to 8 chars) + space
                                //            Len += MD5LEN + 1 + SIZEOFFILESIZE + 1;
                                //        }

                                //        // Add one new line character per file/folder/link -name
                                //        Len++;
                                //    }
                                //}

                                GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

                                pTxBuf->pFile->Size = Len;

                                // Total list length has been calculated
                                pReplyBeginList->ListSizeLsb = (UBYTE)Len;
                                pReplyBeginList->ListSizeNsb1 = (UBYTE)(Len >> 8);
                                pReplyBeginList->ListSizeNsb2 = (UBYTE)(Len >> 16);
                                pReplyBeginList->ListSizeMsb = (UBYTE)(Len >> 24);

                                pTxBuf->MsgLen = BytesToRead;
                                pTxBuf->SendBytes = 0;
                                pTxBuf->pFile->Pointer = 0;
                                pTxBuf->pFile->Length = 0;

                                if (BytesToRead > Len)
                                {
                                    // if message length  > file size then crop message
                                    pTxBuf->MsgLen = Len;
                                    pReplyBeginList->Status = END_OF_FILE; // Complete file in first message
                                }

                                TmpN = pTxBuf->pFile->File;          // Make a copy of number of entries

                                if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINLIST) <= pTxBuf->BufSize)
                                {
                                    //All requested bytes can be inside the buffer
                                    BytesToSend = pTxBuf->MsgLen;
                                    pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYBEGINLIST;
                                    pTxBuf->State = TXIDLE;
                                }
                                else
                                {
                                    BytesToSend = (pTxBuf->BufSize - SIZEOF_RPLYBEGINLIST);
                                    pTxBuf->BlockLen = pTxBuf->BufSize;
                                    pTxBuf->State = TXLISTFILES;
                                }

                                Repeat = 1;
                                Len = 0;
                                pDstAdr = pReplyBeginList->PayLoad;
                                while (Repeat != 0)
                                {
                                    TmpN--;

									// TODO: some files shite
									//cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (DATA8*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

									//if (0 != NameLen)
									//{
									//    if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
									//    {

									//        pSrcAdr = (UBYTE*)(TmpFileName);
									//        while (*pSrcAdr != 0) *pDstAdr++ = *pSrcAdr++;
									//        Len += NameLen;

									//        free(pTxBuf->pFile->namelist[TmpN]);

									//        if (BytesToSend == Len)
									//        {
									//            // buffer is filled up now exit
									//            pTxBuf->pFile->Length = 0;
									//            pTxBuf->pFile->File = TmpN;
									//            Repeat = 0;
									//        }
									//    }
									//    else
									//    {
									//        // No - Now fill up to exact size
									//        pTxBuf->pFile->Length = (BytesToSend - Len);
									//        CommonHelper.memcpy((UBYTE*)pDstAdr, (UBYTE*)TmpFileName, (int)pTxBuf->pFile->Length);
									//        Len += pTxBuf->pFile->Length;
									//        pTxBuf->pFile->File = TmpN;                         // Adjust Iteration number
									//        Repeat = 0;
									//    }
									//}

									GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								}

                                // Update pointers
                                pTxBuf->SendBytes = Len;
                                pTxBuf->pFile->Pointer = Len;

                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
                                {
                                    GH.printf($"Complete list of {(ulong)pTxBuf->pFile->Length} Bytes uploaded \r\n");

                                    pReplyBeginList->Status = END_OF_FILE;
                                    cComFreeHandle(FileHandle);
                                }
                            }
                        }
                        else
                        {
                            // No more handle or illegal file name
                            pReplyBeginList->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyBeginList->Status = UNKNOWN_ERROR;
                            pReplyBeginList->ListSizeLsb = 0;
                            pReplyBeginList->ListSizeNsb1 = 0;
                            pReplyBeginList->ListSizeNsb2 = 0;
                            pReplyBeginList->ListSizeMsb = 0;
                            pReplyBeginList->Handle = byte.MaxValue;

                            pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
                        }
                        pReplyBeginList->CmdSize += (ushort)pTxBuf->MsgLen;

                        cComPrintTxMsg(pTxBuf);
                    }
                    break;

                case CONTINUE_LIST_FILES:
                    {
                        SLONG TmpN;
                        ULONG BytesToRead;
                        ULONG Len = 0;
                        ULONG BytesToSend;
                        ULONG NameLen;
                        ULONG RemCharCnt;
                        UBYTE Repeat;
                        DATA8* TmpFileName = CommonHelper.Pointer1d<DATA8>(FILENAMESIZE);

                        CONTINUE_LIST* pContinueList;
                        RPLY_CONTINUE_LIST* pReplyContinueList;

                        //Setup pointers
                        pContinueList = (CONTINUE_LIST*)pRxBuf->Buf;
                        pReplyContinueList = (RPLY_CONTINUE_LIST*)pTxBuf->Buf;

                        pReplyContinueList->CmdSize = SIZEOF_RPLYCONTINUELIST - sizeof(CMDSIZE);
                        pReplyContinueList->MsgCount = pContinueList->MsgCount;
                        pReplyContinueList->CmdType = SYSTEM_REPLY;
                        pReplyContinueList->Cmd = CONTINUE_LIST_FILES;
                        pReplyContinueList->Handle = pContinueList->Handle;
                        pReplyContinueList->Status = SUCCESS;

                        BytesToRead = (ULONG)pContinueList->BytesToReadLsb;
                        BytesToRead += (ULONG)pContinueList->BytesToReadMsb << 8;

                        if (((pTxBuf->pFile->File == 0) && (0 > pTxBuf->pFile->Length)) || (pTxBuf->pFile->File < 0))
                        {
                            // here if nothing is to be returned
                            pReplyContinueList->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyContinueList->Status = UNKNOWN_ERROR;
                            pReplyContinueList->Handle = byte.MaxValue;

                            pTxBuf->MsgLen = 0;
                            pTxBuf->BlockLen = SIZEOF_RPLYCONTINUELIST;
                            cComFreeHandle(FileHandle);
                        }
                        else
                        {
                            TmpN = pTxBuf->pFile->File;       // Make a copy of number of entries

                            pTxBuf->MsgLen = BytesToRead;
                            pTxBuf->SendBytes = 0;

                            if (BytesToRead >= (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer))
                            {
                                // if message length  > file size then crop message
                                pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
                                pReplyContinueList->Status = END_OF_FILE;           // Remaining file included in this message
                            }

                            if ((pTxBuf->MsgLen + SIZEOF_RPLYCONTINUELIST) <= pTxBuf->BufSize)
                            {
                                BytesToSend = pTxBuf->MsgLen;
                                pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYCONTINUELIST;
                                pTxBuf->State = TXIDLE;
                            }
                            else
                            {
                                BytesToSend = pTxBuf->BufSize - SIZEOF_RPLYCONTINUELIST;
                                pTxBuf->BlockLen = pTxBuf->BufSize;
                                pTxBuf->State = TXLISTFILES;
                            }

                            Len = 0;
                            Repeat = 1;
                            if (pTxBuf->pFile->Length != 0)
                            {

								// TODO: some shite with files
								// Only here if filename has been divided in 2 pieces
								//cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (DATA8*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

								//if (0 != NameLen)
								//{
								//    // First transfer the remaining part of the last filename
								//    RemCharCnt = NameLen - pTxBuf->pFile->Length;

								//    if (RemCharCnt <= BytesToSend)
								//    {
								//        // this will fit into the message length
								//        CommonHelper.memcpy((UBYTE*)(&(pReplyContinueList->PayLoad[Len])), (UBYTE*)&(TmpFileName[pTxBuf->pFile->Length]), (int)RemCharCnt);
								//        Len += RemCharCnt;

								//        free(pTxBuf->pFile->namelist[TmpN]);

								//        if (RemCharCnt == BytesToSend)
								//        {
								//            //if If all bytes is already occupied not more to go
								//            //for this message
								//            Repeat = 0;
								//            pTxBuf->pFile->Length = 0;
								//        }
								//        else
								//        {
								//            Repeat = 1;
								//        }
								//    }
								//    else
								//    {
								//        // This is the rare condition if remaining msg len and buf size are almost equal
								//        CommonHelper.memcpy((UBYTE*)(&(pReplyContinueList->PayLoad[Len])), (UBYTE*)&(TmpFileName[pTxBuf->pFile->Length]), (int)BytesToSend);
								//        Len += BytesToSend;

								//        pTxBuf->pFile->File = TmpN;                         // Adjust Iteration number
								//        pTxBuf->pFile->Length += Len;
								//        Repeat = 0;
								//    }
								//}

								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}
                            if (TmpN != 0)
                            {
                                while (Repeat != 0)
                                {
                                    TmpN--;

									// TODO: some shite with files
									//cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (DATA8*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

									//if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
									//{
									//    CommonHelper.memcpy((UBYTE*)(&(pReplyContinueList->PayLoad[Len])), (UBYTE*)TmpFileName, (int)NameLen);
									//    Len += NameLen;

									//    free(pTxBuf->pFile->namelist[TmpN]);

									//    if (BytesToSend == Len)
									//    {
									//        // buffer is filled up now exit
									//        pTxBuf->pFile->Length = 0;
									//        pTxBuf->pFile->File = TmpN;
									//        Repeat = 0;
									//    }
									//}
									//else
									//{
									//    // Now fill up to complete buffer size
									//    pTxBuf->pFile->Length = (BytesToSend - Len);
									//    CommonHelper.memcpy((UBYTE*)(&(pReplyContinueList->PayLoad[Len])), (UBYTE*)TmpFileName, (int)pTxBuf->pFile->Length);
									//    Len += pTxBuf->pFile->Length;
									//    pTxBuf->pFile->File = TmpN;         // Adjust Iteration number
									//    Repeat = 0;
									//}

									GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								}
                            }
                        }

                        // Update pointers
                        pTxBuf->SendBytes = Len;
                        pTxBuf->pFile->Pointer += Len;

                        if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
                        {
                            GH.printf($"Complete list of {(ulong)pTxBuf->pFile->Length} Bytes uploaded \r\n");

                            pReplyContinueList->Status = END_OF_FILE;
                            cComFreeHandle((sbyte)pContinueList->Handle);
                        }

                        pReplyContinueList->CmdSize += (ushort)pTxBuf->MsgLen;

                        cComPrintTxMsg(pTxBuf);
                    }
                    break;

                case CLOSE_FILEHANDLE:
                    {
                        CLOSE_HANDLE* pCloseHandle;
                        RPLY_CLOSE_HANDLE* pReplyCloseHandle;

                        //Setup pointers
                        pCloseHandle = (CLOSE_HANDLE*)pRxBuf->Buf;
                        pReplyCloseHandle = (RPLY_CLOSE_HANDLE*)pTxBuf->Buf;

                        FileHandle = (sbyte)pCloseHandle->Handle;

                        GH.printf($"FileHandle to close = {FileHandle}, Linux Handle = {GH.ComInstance.Files[FileHandle].File}\r\n");

                        pReplyCloseHandle->CmdSize = SIZEOF_RPLYCLOSEHANDLE - sizeof(CMDSIZE);
                        pReplyCloseHandle->MsgCount = pCloseHandle->MsgCount;
                        pReplyCloseHandle->CmdType = SYSTEM_REPLY;
                        pReplyCloseHandle->Cmd = CLOSE_FILEHANDLE;
                        pReplyCloseHandle->Handle = pCloseHandle->Handle;
                        pReplyCloseHandle->Status = SUCCESS;

                        if (1 == cComFreeHandle(FileHandle))
                        {
                            cComCloseFileHandle(&(GH.ComInstance.Files[FileHandle].File));
                        }
                        else
                        {
                            pReplyCloseHandle->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyCloseHandle->Status = UNKNOWN_HANDLE;
                        }
                        pTxBuf->BlockLen = SIZEOF_RPLYCLOSEHANDLE;

                        cComPrintTxMsg(pTxBuf);
                    }
                    break;

                case CREATE_DIR:
                    {
                        MAKE_DIR* pMakeDir;
                        RPLY_MAKE_DIR* pReplyMakeDir;
                        DATA8* FolderLocal = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

                        //Setup pointers
                        pMakeDir = (MAKE_DIR*)pRxBuf->Buf;
                        pReplyMakeDir = (RPLY_MAKE_DIR*)pTxBuf->Buf;

                        pReplyMakeDir->CmdSize = SIZEOF_RPLYMAKEDIR - sizeof(CMDSIZE);
                        pReplyMakeDir->MsgCount = pMakeDir->MsgCount;
                        pReplyMakeDir->CmdType = SYSTEM_REPLY;
                        pReplyMakeDir->Cmd = CREATE_DIR;
                        pReplyMakeDir->Status = SUCCESS;

                        CommonHelper.snprintf(FolderLocal, vmFILENAMESIZE, CommonHelper.GetString((DATA8*)(pMakeDir->Dir)));

                        if (Directory.CreateDirectory(CommonHelper.GetString(FolderLocal)) != null)
                        {
                            GH.printf($"Folder {CommonHelper.GetString(FolderLocal)} created\r\n");
                            GH.Lms.SetUiUpdate();
                        }
                        else
                        {
                            pReplyMakeDir->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyMakeDir->Status = NO_PERMISSION;

                            GH.printf($"Folder {CommonHelper.GetString(FolderLocal)} not created (%s)\r\n");
                        }
                        pTxBuf->BlockLen = SIZEOF_RPLYMAKEDIR;
                    }
                    break;

                case DELETE_FILE:
                    {
                        REMOVE_FILE* pRemove;
                        RPLY_REMOVE_FILE* pReplyRemove;
                        DATA8* Name = CommonHelper.Pointer1d<DATA8>(60);

                        //Setup pointers
                        pRemove = (REMOVE_FILE*)pRxBuf->Buf;
                        pReplyRemove = (RPLY_REMOVE_FILE*)pTxBuf->Buf;

                        pReplyRemove->CmdSize = SIZEOF_RPLYREMOVEFILE - sizeof(CMDSIZE);
                        pReplyRemove->MsgCount = pRemove->MsgCount;
                        pReplyRemove->CmdType = SYSTEM_REPLY;
                        pReplyRemove->Cmd = DELETE_FILE;
                        pReplyRemove->Status = SUCCESS;

                        CommonHelper.snprintf(Name, 60, CommonHelper.GetString((DATA8*)(pRemove->Name)));

                        GH.printf($"File to delete {CommonHelper.GetString(Name)}\r\n");
                        
                        if (File.Exists(CommonHelper.GetString(Name)))
                        {
                            // this is a file
                            File.Delete(CommonHelper.GetString(Name));
							if (true)
							{
								GH.Lms.SetUiUpdate();
							}
							else
							{
								pReplyRemove->CmdType = SYSTEM_REPLY_ERROR;
								pReplyRemove->Status = NO_PERMISSION;

								GH.printf($"File {CommonHelper.GetString(Folder)} not deleted (%s)\r\n");
							}
						}
                        else
                        {
							// this is a folder
							Directory.Delete(CommonHelper.GetString(Name), true);
							if (true)
							{
								GH.Lms.SetUiUpdate();
							}
							else
							{
								pReplyRemove->CmdType = SYSTEM_REPLY_ERROR;
								pReplyRemove->Status = NO_PERMISSION;

								GH.printf($"Folder {CommonHelper.GetString(Folder)} not deleted (%s)\r\n");
							}
						}
                        
                        pTxBuf->BlockLen = SIZEOF_RPLYREMOVEFILE;
                    }
                    break;

                case LIST_OPEN_HANDLES:
                    {
                        UBYTE HCnt1, HCnt2;

                        LIST_HANDLES* pListHandles;
                        RPLY_LIST_HANDLES* pReplyListHandles;

                        //Setup pointers
                        pListHandles = (LIST_HANDLES*)pRxBuf->Buf;
                        pReplyListHandles = (RPLY_LIST_HANDLES*)pTxBuf->Buf;

                        pReplyListHandles->CmdSize = SIZEOF_RPLYLISTHANDLES - sizeof(CMDSIZE);
                        pReplyListHandles->MsgCount = pListHandles->MsgCount;
                        pReplyListHandles->CmdType = SYSTEM_REPLY;
                        pReplyListHandles->Cmd = LIST_OPEN_HANDLES;
                        pReplyListHandles->Status = SUCCESS;

                        for (HCnt1 = 0; HCnt1 < ((MAX_FILE_HANDLES / 8) + 1); HCnt1++)
                        {

                            pReplyListHandles->PayLoad[HCnt1 + 2] = 0;

                            for (HCnt2 = 0; HCnt2 < 8; HCnt2++)
                            {

                                if (0 != GH.ComInstance.Files[HCnt2 * HCnt1].State)
                                { // Filehandle is in use

                                    pReplyListHandles->PayLoad[HCnt1 + 2] |= (byte)(0x01 << HCnt2);
                                }
                            }
                        }
                        pReplyListHandles->CmdSize += HCnt1;
                        pTxBuf->BlockLen = (uint)(SIZEOF_RPLYLISTHANDLES + HCnt1);
                    }
                    break;

                case WRITEMAILBOX:
                    {
                        UBYTE No;
                        UWORD PayloadSize;
                        WRITE_MAILBOX* pWriteMailbox;
                        WRITE_MAILBOX_PAYLOAD* pWriteMailboxPayload;

                        pWriteMailbox = (WRITE_MAILBOX*)pRxBuf->Buf;

                        if (1 == cComFindMailbox(&(pWriteMailbox->Name[0]), &No))
                        {
                            pWriteMailboxPayload = (WRITE_MAILBOX_PAYLOAD*)&(pWriteMailbox->Name[(pWriteMailbox->NameSize)]);
                            PayloadSize = (UWORD)(pWriteMailboxPayload->SizeLsb);
                            PayloadSize += (UWORD)(((UWORD)(pWriteMailboxPayload->SizeMsb)) << 8);
                            CommonHelper.memcpy((byte*)GH.ComInstance.MailBox[No].Content, pWriteMailboxPayload->Payload, PayloadSize);
                            GH.ComInstance.MailBox[No].DataSize = PayloadSize;
                            GH.ComInstance.MailBox[No].WriteCnt++;
                        }
                    }
                    break;

                case BLUETOOTHPIN:
                    {
                        // Both MAC and Pin are zero terminated string type
                        UBYTE* BtAddr = CommonHelper.Pointer1d<UBYTE>(vmBTADRSIZE);
                        UBYTE* Pin = CommonHelper.Pointer1d<UBYTE>(vmBTPASSKEYSIZE);
                        UBYTE PinSize;
                        BLUETOOTH_PIN* pBtPin;
                        RPLY_BLUETOOTH_PIN* pReplyBtPin;

                        pBtPin = (BLUETOOTH_PIN*)pRxBuf->Buf;
                        pReplyBtPin = (RPLY_BLUETOOTH_PIN*)pTxBuf->Buf;

                        CommonHelper.snprintf((DATA8*)BtAddr, pBtPin->MacSize, CommonHelper.GetString((DATA8*)pBtPin->Mac));
                        PinSize = pBtPin->PinSize;
                        CommonHelper.snprintf((DATA8*)Pin, PinSize, CommonHelper.GetString((DATA8*)pBtPin->Pin));

                        // This command can for safety reasons only be handled by USB
                        if (USBDEV == GH.ComInstance.ActiveComCh)
                        {
                            GH.Bt.cBtSetTrustedDev(BtAddr, Pin, PinSize);
                            pReplyBtPin->Status = SUCCESS;
                        }
                        else
                        {
                            pReplyBtPin->Status = ILLEGAL_CONNECTION;
                        }

                        pReplyBtPin->CmdSize = 0x00;
                        pReplyBtPin->MsgCount = pBtPin->MsgCount;
                        pReplyBtPin->CmdType = SYSTEM_REPLY;
                        pReplyBtPin->Cmd = BLUETOOTHPIN;
                        pReplyBtPin->MacSize = vmBTADRSIZE;

						GH.Bt.cBtGetId(pReplyBtPin->Mac, vmBTADRSIZE);
                        pReplyBtPin->PinSize = PinSize;
                        CommonHelper.memcpy(pReplyBtPin->Pin, pBtPin->Pin, PinSize);

						pReplyBtPin->CmdSize = (ushort)((SIZEOF_RPLYBLUETOOTHPIN + pReplyBtPin->MacSize + pReplyBtPin->PinSize + 1 + 1) - sizeof(CMDSIZE));
                        pTxBuf->BlockLen = (uint)((pReplyBtPin->CmdSize) + sizeof(CMDSIZE));
                    }
                    break;

                case ENTERFWUPDATE:
                    {
                        ULONG UpdateFile;
                        UBYTE Dummy;

                        if (USBDEV == GH.ComInstance.ActiveComCh)
                        {
                            // TODO: do i need fw update?
                            //UpdateFile = open(UPDATE_DEVICE_NAME, O_RDWR);

                            //if (UpdateFile >= 0)
                            //{
                            //    write(UpdateFile, &Dummy, 1);
                            //    close(UpdateFile);
                            //    system("reboot -d -f -i");
                            //}
                        }
                    }
                    break;

                case SETBUNDLEID:
                    {
                        BUNDLE_ID* pBundleId;
                        RPLY_BUNDLE_ID* pReplyBundleId;

                        pBundleId = (BUNDLE_ID*)pRxBuf->Buf;
                        pReplyBundleId = (RPLY_BUNDLE_ID*)pTxBuf->Buf;

                        pReplyBundleId->CmdSize = 0x05;
                        pReplyBundleId->MsgCount = pBundleId->MsgCount;
                        pReplyBundleId->Cmd = SETBUNDLEID;

                        if (1 == GH.Bt.cBtSetBundleId(pBundleId->BundleId))
                        {
                            // Success
                            pReplyBundleId->CmdType = SYSTEM_REPLY;
                            pReplyBundleId->Status = SUCCESS;
                        }
                        else
                        {
                            // Error
                            pReplyBundleId->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyBundleId->Status = SIZE_ERROR;
                        }
                        pTxBuf->BlockLen = SIZEOF_RPLYBUNDLEID;
                    }
                    break;

                case SETBUNDLESEEDID:
                    {
                        BUNDLE_SEED_ID* pBundleSeedId;
                        RPLY_BUNDLE_SEED_ID* pReplyBundleSeedId;

                        pBundleSeedId = (BUNDLE_SEED_ID*)pRxBuf->Buf;
                        pReplyBundleSeedId = (RPLY_BUNDLE_SEED_ID*)pTxBuf->Buf;

                        pReplyBundleSeedId->CmdSize = 0x05;
                        pReplyBundleSeedId->MsgCount = pBundleSeedId->MsgCount;
                        pReplyBundleSeedId->Cmd = SETBUNDLESEEDID;

                        if (1 == GH.Bt.cBtSetBundleSeedId(pBundleSeedId->BundleSeedId))
                        {
                            // Success
                            pReplyBundleSeedId->CmdType = SYSTEM_REPLY;
                            pReplyBundleSeedId->Status = SUCCESS;
                        }
                        else
                        {
                            // Error
                            pReplyBundleSeedId->CmdType = SYSTEM_REPLY_ERROR;
                            pReplyBundleSeedId->Status = SIZE_ERROR;
                        }
                        pTxBuf->BlockLen = SIZEOF_RPLYBUNDLESEEDID;
                    }
                    break;
            }
        }


        public void cComUpdate()
        {
            COMCMD* pComCmd;
            IMGHEAD* pImgHead;
            TXBUF* pTxBuf;
            RXBUF* pRxBuf;
            UBYTE ChNo;
            UWORD BytesRead;

            UBYTE ThisMagicCookie;
            UBYTE HelperByte;
            uint Iterator;
            UBYTE MotorBusySignal;
            UBYTE MotorBusySignalPointer;

            ChNo = 0;

			GH.Daisy.cDaisyControl();  // Keep the HOST part going

            for (ChNo = 0; ChNo < NO_OF_CHS; ChNo++)
            {

                pTxBuf = &(GH.ComInstance.TxBuf[ChNo]);
                pRxBuf = &(GH.ComInstance.RxBuf[ChNo]);
                BytesRead = 0;

                if (pTxBuf->Writing == 0)
                {
                    if (null != GH.ComInstance.ReadChannel[ChNo])
                    {
                        if (GH.ComInstance.VmReady == 1)
                        {
                            BytesRead = GH.ComInstance.ReadChannel[ChNo](pRxBuf->Buf, (ushort)pRxBuf->BufSize);
                        }
                    }

                    // start DEBUG
                    if (ChNo == USBDEV)
                    {
                        GH.printf($"Writing NOT set @ USBDEV - BytesRead = {BytesRead}\r\n");
                    }
                    // end DEBUG


                    if (BytesRead != 0)
                    {
                        // Temporary fix until full implementation of com channels is ready
                        GH.ComInstance.ActiveComCh = ChNo;

                        if (RXIDLE == pRxBuf->State)
                        {
                            // Not file down-loading
                            CommonHelper.memset(pTxBuf->Buf, 0, 1024);

                            pComCmd = (COMCMD*)pRxBuf->Buf;

                            if ((*pComCmd).CmdSize != 0)
                            {
                                // message received
                                switch ((*pComCmd).Cmd)
                                {
                                    // NEW MOTOR/DAISY
                                    case DIR_CMD_NO_REPLY_WITH_BUSY:
                                        {
                                            GH.printf($"Did we reach a *BUSY* DIRECT_COMMAND_NO_REPLY pComCmd.Size = {((*pComCmd).CmdSize)}\n\r");

                                            Iterator = (uint)(((*pComCmd).CmdSize) - 7); // Incl. LEN bytes

                                            HelperByte = 0x00;

                                            for (Iterator = ((uint)(((*pComCmd).CmdSize) - 7)); Iterator < (((*pComCmd).CmdSize) - 3); Iterator++)
                                            {
                                                HelperByte |= (byte)((*pComCmd).PayLoad[Iterator] & 0x03);
                                            }

                                            GH.Daisy.cDaisySetOwnLayer(HelperByte);

                                            // Setup the Cookie stuff get one by one and store

                                            HelperByte = 0; // used as Index

                                            // New MotorSignal
                                            MotorBusySignal = 0;
                                            MotorBusySignalPointer = 1;

                                            for (Iterator = ((uint)(((*pComCmd).CmdSize) - 7)); Iterator < (((*pComCmd).CmdSize) - 3); Iterator++)
                                            {
                                                GH.printf($"Iterator = {Iterator}\n\r");

                                                ThisMagicCookie = (*pComCmd).PayLoad[Iterator];

                                                // New MotorSignal
                                                if ((ThisMagicCookie & 0x80) != 0) // BUSY signalled
                                                {
                                                    MotorBusySignal = (UBYTE)(MotorBusySignal | MotorBusySignalPointer);
                                                }

                                                // New MotorSignal
                                                MotorBusySignalPointer <<= 1;

                                                GH.printf($"ThisMagicCookie = {ThisMagicCookie}\n\r");

												GH.Daisy.cDaisySetBusyFlags(GH.Daisy.cDaisyGetOwnLayer(), HelperByte, ThisMagicCookie);
                                                HelperByte++;
                                            }

                                            GH.Output.ResetDelayCounter(MotorBusySignal);

                                            GH.printf($"cMotorSetBusyFlags({MotorBusySignal})\n\r");

											// New MotorSignal
											GH.Output.cMotorSetBusyFlags(MotorBusySignal);

                                            // Adjust length

                                            ((*pComCmd).CmdSize) -= 4;  // NO VM use of cookies (or sweets ;-))

                                            // Now as "normal" direct command with NO reply

                                            GH.ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
                                        }
                                        break;

                                    case DIRECT_COMMAND_REPLY:
                                        {
                                            // direct command
                                            GH.printf("Did we reach a DIRECT_COMMAND_REPLY\n\r");

                                            if (0 == GH.ComInstance.ReplyStatus)
                                            {
                                                // If ReplyStstus = 0 then no commands is currently being
                                                // processed -> new command can be processed
                                                GH.ComInstance.ReplyStatus |= DIR_CMD_REPLY;
                                                GH.ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
                                                if (GH.ComInstance.CommandReady == 0)
                                                {
                                                    // some error
                                                    pTxBuf->Writing = 1;
                                                    GH.ComInstance.ReplyStatus = 0;
                                                }
                                            }
                                            else
                                            {
                                                // Else VM is currently processing direct commands
                                                // send error command, only if a DIR_CMD_NOREPLY
                                                // is being executed. Not if a DIR_CMD_NOREPLY is
                                                // being executed as replies can collide.
                                                if ((DIR_CMD_NOREPLY & GH.ComInstance.ReplyStatus) != 0)
                                                {
                                                    COMCMD* pComCmdLocal;
                                                    COMRPL* pComRpl;

													pComCmdLocal = (COMCMD*)pRxBuf->Buf;
                                                    pComRpl = (COMRPL*)pTxBuf->Buf;

                                                    (*pComRpl).CmdSize = 3;
                                                    (*pComRpl).MsgCnt = (*pComCmdLocal).MsgCnt;
                                                    (*pComRpl).Cmd = DIRECT_REPLY_ERROR;

                                                    pTxBuf->Writing = 1;
                                                }
                                            }
                                        }
                                        break;

                                    case DIRECT_COMMAND_NO_REPLY:
                                        {
                                            // direct command

                                            GH.printf("Did we reach a DIRECT_COMMAND_NO_REPLY\n\r");

                                            //Do not reply even if error
                                            if (0 == GH.ComInstance.ReplyStatus)
                                            {
                                                // If ReplyStstus = 0 then no commands is currently being
                                                // processed -> new command can be processed
                                                GH.ComInstance.ReplyStatus |= DIR_CMD_NOREPLY;
                                                GH.ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
                                            }
                                        }
                                        break;

                                    case SYSTEM_COMMAND_REPLY:
                                        {
                                            if (0 == GH.ComInstance.ReplyStatus)
                                            {
                                                GH.ComInstance.ReplyStatus |= SYS_CMD_REPLY;
                                                cComSystemCommand(pRxBuf, pTxBuf);
                                                if (RXFILEDL != pRxBuf->State)
                                                {
                                                    // Only here if command has been completed
                                                    pTxBuf->Writing = 1;
                                                    GH.ComInstance.ReplyStatus = 0;
                                                }
                                            }
                                        }
                                        break;

                                    case SYSTEM_COMMAND_NO_REPLY:
                                        {
                                            if (0 == GH.ComInstance.ReplyStatus)
                                            {
                                                GH.ComInstance.ReplyStatus |= SYS_CMD_NOREPLY;
                                                cComSystemCommand(pRxBuf, pTxBuf);
                                                if (RXFILEDL != pRxBuf->State)
                                                {
                                                    // Only here if command has been completed
                                                    GH.ComInstance.ReplyStatus = 0;
                                                }
                                            }
                                        }
                                        break;

                                    case SYSTEM_REPLY:
                                        {
                                            cComSystemReply(pRxBuf, pTxBuf);
                                        }
                                        break;

                                    case SYSTEM_REPLY_ERROR:
                                        {
                                            GH.printf("\r\nsystem reply error\r\n");
                                        }
                                        break;

                                    case DAISY_COMMAND_REPLY:
                                        {
                                            if (ChNo == USBDEV)
                                            {
                                                GH.printf("Did we reach c_COM @ DAISY_COMMAND_REPLY?\n\r");

                                                GH.Daisy.cDaisyCmd(pRxBuf, pTxBuf);

                                            }
                                            else
                                            {
                                                // Some ERROR handling
                                            }
                                        }
                                        break;

                                    case DAISY_COMMAND_NO_REPLY:
                                        {

                                            GH.printf("Did we reach c_COM @ DAISY_COMMAND_NO_REPLY?\n\r");

                                            // A Daisy command without any reply
                                            if (ChNo == USBDEV)
                                            {
                                                // Do something
                                                GH.Daisy.cDaisyCmd(pRxBuf, pTxBuf);
                                            }
                                            else
                                            {
                                                // Some ERROR handling
                                            }
                                        }
                                        break;

                                    default:
                                        {
                                        }
                                        break;

                                }
                            }
                            else
                            { // poll received
                              // send response

                                pImgHead = (IMGHEAD*)GH.ComInstance.Image;
                                pComCmd = (COMCMD*)pTxBuf->Buf;

                                (*pComCmd).CmdSize = (ushort)((CMDSIZE)(*pImgHead).GlobalBytes + 1);
                                (*pComCmd).Cmd = DIRECT_REPLY;
                                CommonHelper.memcpy((*pComCmd).PayLoad, GH.ComInstance.Globals, (int)(*pImgHead).GlobalBytes);

                                pTxBuf->Writing = 1;
                            }
                        }
                        else
                        { // in the middle of a write file command
                            ULONG RemBytes;
                            ULONG BytesToWrite;

                            RemBytes = pRxBuf->MsgLen - pRxBuf->RxBytes;

                            if (RemBytes <= pRxBuf->BufSize)
                            {
                                // Remaining bytes to write
                                BytesToWrite = RemBytes;

                                // Send the reply if requested
                                if ((GH.ComInstance.ReplyStatus & SYS_CMD_REPLY) != 0)
                                {
                                    pTxBuf->Writing = 1;
                                }

                                // Clear to receive next msg header
                                pRxBuf->State = RXIDLE;
                                GH.ComInstance.ReplyStatus = 0;
                            }
                            else
                            {
                                BytesToWrite = pRxBuf->BufSize;
                            }

							// TODO: some files shite
							//write(pRxBuf->pFile->File, pRxBuf->Buf, (size_t)BytesToWrite);
							//pRxBuf->pFile->Pointer += (ULONG)BytesToWrite;
							//pRxBuf->RxBytes += (ULONG)BytesToWrite;

							//if (pRxBuf->pFile->Pointer >= pRxBuf->pFile->Size)
							//{
							//    cComCloseFileHandle(&(pRxBuf->pFile->File));
							//    chmod(pRxBuf->pFile->Name, S_IRWXU | S_IRWXG | S_IRWXO);
							//    cComFreeHandle((sbyte)pRxBuf->FileHandle);
							//}

							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
						}
                    }
                }
                cComTxUpdate(ChNo);

                // Time for USB unplug detect?
                UsbConUpdate++;
                if (UsbConUpdate >= USB_CABLE_DETECT_RATE)
                {
                    GH.printf("ready to check\n\r");

                    UsbConUpdate = 0;
                    cComUsbDeviceConnected = cComCheckUsbCable();
                }

            }

            GH.Bt.BtUpdate();
			GH.Wifi.cWiFiControl();
        }

        public DATA8 cComGetUsbStatus()
        {
            // Returns true if the USB port is connected
            return (sbyte)cComUsbDeviceConnected;
        }

        public void cComTxUpdate(UBYTE ChNo)
        {
            TXBUF* pTxBuf;
            ULONG ReadBytes = 0;

            pTxBuf = &(GH.ComInstance.TxBuf[ChNo]);

            if ((OK == GH.Daisy.cDaisyChained()) && (ChNo == USBDEV)) // We're part of a chain && USBDEV
            {
                // Do "special handling" - I.e. no conflict between pushed DaisyData and non-syncronized
                // returns from Commands (answers/errors).

                GH.printf("\n\r001\n\r");

                if (GH.Daisy.GetDaisyPushCounter() == DAISY_PUSH_NOT_UNLOCKED)  // It's the very first
                {                                                     // (or one of the first ;-)) transmission(s)
                                                                      //#define DEBUG
                    GH.printf("Not unlocked 001\n\r");

                    if (pTxBuf->Writing != 0)  // Anything Pending?
                    {
                        //#define DEBUG
                        GH.printf("Not unlocked 002\n\r");

                        if (null != GH.ComInstance.WriteChannel[ChNo])  // Valid channel?
                        {
                            GH.printf("Not unlocked 003\n\r");

                            if ((GH.ComInstance.WriteChannel[USBDEV](pTxBuf->Buf, (ushort)pTxBuf->BlockLen)) != 0)
                            {
                                GH.printf("Not (OR should be) unlocked 004\n\r");

                                pTxBuf->Writing = 0;

								GH.Daisy.ResetDaisyPushCounter();  // Ready for normal run

                            }
                        }
                    }
                }
                else
                {
                    // We're unlocked

                    if (GH.Daisy.GetDaisyPushCounter() == 0)  // It's a NON DaisyChain time-slice
                    {
                        //#define DEBUG
                        GH.printf("Unlocked 001\n\r");

                        if (pTxBuf->Writing != 0)
                        {
                            GH.printf("Unlocked 002\n\r");

                            if (null != GH.ComInstance.WriteChannel[ChNo])
                            {
                                GH.printf("Unlocked 003\n\r");

                                if (GH.ComInstance.WriteChannel[ChNo](pTxBuf->Buf, (ushort)pTxBuf->BlockLen) != 0)
                                {
                                    GH.printf("Unlocked 004\n\r");

                                    pTxBuf->Writing = 0;
									GH.Daisy.ResetDaisyPushCounter();  // Done/or we'll wait - we can allow more Daisy stuff
                                }

                            }
                        }
                        else
                        {
                            GH.printf("Unlocked 005\n\r");

							GH.Daisy.ResetDaisyPushCounter();      // Skip this "master" slice - use time with more benefit ;-)
                        }
                    }
                    else
                    {
                        // We have a prioritised Daisy transfer/time-slice
                        // DaisyPushCounter == either 3, 2 or 1

                        if (null != GH.ComInstance.WriteChannel[USBDEV])
                        {
                            UBYTE* pData;     // Pointer to dedicated Daisy Upstream Buffer (INFO or Data)
                            UWORD Len = 0;

                            Len = GH.Daisy.cDaisyData(&pData);

                            GH.printf($"Daisy Len = {Len}, Counter = {GH.Daisy.GetDaisyPushCounter()}\n\r");

                            if (Len > 0)
                            {

                                if ((GH.ComInstance.WriteChannel[USBDEV](pData, Len)) != 0)
                                {
                                    GH.printf($"Daisy OK tx{GH.Daisy.GetDaisyPushCounter()}\n\r");

									GH.Daisy.DecrementDaisyPushCounter();
									GH.Daisy.cDaisyPushUpStream(); // Flood upward
									GH.Daisy.cDaisyPrepareNext();  // Ready for the next sensor in the array
                                }
                                else
                                {
                                    GH.printf($"Daisy RESULT.FAIL in txing {GH.Daisy.GetDaisyPushCounter()}\n\r");  // TX upstream called
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                if (pTxBuf->Writing != 0)
                {
                    GH.printf("007\n\r");

                    if (null != GH.ComInstance.WriteChannel[ChNo])
                    {
                        GH.printf($"Tx Writing true in the bottom ChNo = {ChNo} - PushCounter = {GH.Daisy.GetDaisyPushCounter()}\n\r");

                        if (GH.ComInstance.WriteChannel[ChNo](pTxBuf->Buf, (ushort)pTxBuf->BlockLen) != 0)
                        {
                            GH.printf("008\n\r");

                            pTxBuf->Writing = 0;
                        }
                    }

                }
            }

            if (0 == pTxBuf->Writing)
            {
                // Tx buffer needs to be empty to fill new data into it....
                switch (pTxBuf->State)
                {
                    case TXFILEUPLOAD:
                        {
                            ULONG MsgLeft;

                            MsgLeft = pTxBuf->MsgLen - pTxBuf->SendBytes;

							// TODO: some files shite
							//if (MsgLeft > pTxBuf->BufSize)
							//{
							//    ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)pTxBuf->BufSize);
							//    pTxBuf->pFile->Pointer += ReadBytes;
							//    pTxBuf->SendBytes += ReadBytes;
							//    pTxBuf->State = TXFILEUPLOAD;
							//}
							//else
							//{
							//    ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)MsgLeft);
							//    pTxBuf->pFile->Pointer += ReadBytes;
							//    pTxBuf->SendBytes += ReadBytes;

							//    if (pTxBuf->MsgLen == pTxBuf->SendBytes)
							//    {
							//        pTxBuf->State = TXIDLE;

							//        if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
							//        { //All Bytes has been read in the file - close handles (it is not GetFile command)

							//            GH.printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (ulong)pTxBuf->pFile->Length);

							//            cComCloseFileHandle(&(pTxBuf->pFile->File));
							//            cComFreeHandle((sbyte)pTxBuf->FileHandle);
							//        }
							//    }
							//}

							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							if (ReadBytes != 0)
                            {
                                pTxBuf->Writing = 1;
                            }
                        }
                        break;

                    case TXGETFILE:
                        {
                            ULONG MsgLeft;

                            MsgLeft = pTxBuf->MsgLen - pTxBuf->SendBytes;

							// TODO: some files shite
							//if (MsgLeft > pTxBuf->BufSize)
							//{
							//    ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)pTxBuf->BufSize);
							//    pTxBuf->pFile->Pointer += ReadBytes;
							//    pTxBuf->SendBytes += ReadBytes;
							//    pTxBuf->State = TXGETFILE;
							//}
							//else
							//{
							//    ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)MsgLeft);
							//    pTxBuf->pFile->Pointer += ReadBytes;
							//    pTxBuf->SendBytes += ReadBytes;

							//    if (pTxBuf->MsgLen == pTxBuf->SendBytes)
							//    {
							//        pTxBuf->State = TXIDLE;

							//        if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
							//        {

							//            GH.printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (ulong)pTxBuf->pFile->Length);
							//        }
							//    }
							//}
							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							if (ReadBytes != 0)
                            {
                                pTxBuf->Writing = 1;
                            }
                        }
                        break;

                    case TXLISTFILES:
                        {
                            ULONG TmpN;
                            ULONG Len;
                            ULONG NameLen;
                            ULONG RemCharCnt;
                            ULONG BytesToSend;
                            UBYTE Repeat;
                            DATA8* TmpFileName = CommonHelper.Pointer1d<DATA8>(FILENAMESIZE);

                            TmpN = (uint)pTxBuf->pFile->File;
                            Len = 0;
                            Repeat = 1;

                            if ((pTxBuf->MsgLen - pTxBuf->SendBytes) <= pTxBuf->BufSize)
                            {
                                //All requested bytes can be inside the buffer
                                BytesToSend = (pTxBuf->MsgLen - pTxBuf->SendBytes);
                                pTxBuf->State = TXIDLE;
                            }
                            else
                            {
                                BytesToSend = pTxBuf->BufSize;
                                pTxBuf->State = TXLISTFILES;
                            }
                            pTxBuf->BlockLen = BytesToSend;

                            if (pTxBuf->pFile->Length != 0)
                            {
								// TODO: some files shite
								// Only here if filename has been divided in 2 pieces
								// First transfer the remaining part of the last filename
								//cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (DATA8*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);
								//RemCharCnt = NameLen - pTxBuf->pFile->Length;

								//if (RemCharCnt <= BytesToSend)
								//{
								//    // this will fit into the message length
								//    CommonHelper.memcpy((UBYTE*)(&(pTxBuf->Buf[Len])), (UBYTE*)&(TmpFileName[pTxBuf->pFile->Length]), (int)RemCharCnt);
								//    Len += RemCharCnt;

								//    free(pTxBuf->pFile->namelist[TmpN]);

								//    if (RemCharCnt == BytesToSend)
								//    {
								//        //if If all bytes is already occupied not more to go right now
								//        Repeat = 0;
								//        pTxBuf->pFile->Length = 0;
								//    }
								//    else
								//    {
								//        Repeat = 1;
								//    }
								//}
								//else
								//{
								//    // This is the rare condition if remaining msg len and buf size are almost equal
								//    CommonHelper.memcpy((UBYTE*)(&(pTxBuf->Buf[Len])), (UBYTE*)&(TmpFileName[pTxBuf->pFile->Length]), (int)BytesToSend);
								//    Len += BytesToSend;
								//    pTxBuf->pFile->File = (int)TmpN;                         // Adjust Iteration number
								//    pTxBuf->pFile->Length += Len;
								//    Repeat = 0;
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}

                            while (Repeat != 0)
                            {

                                TmpN--;

								// TODO: some files shite
								//cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (DATA8*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

								//if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
								//{
								//    CommonHelper.memcpy((UBYTE*)(&(pTxBuf->Buf[Len])), (UBYTE*)TmpFileName, (int)NameLen);
								//    Len += NameLen;

								//    GH.printf("List entry no = %d; File name = %s \r\n", TmpN, pTxBuf->pFile->namelist[TmpN]->d_name);

								//    free(pTxBuf->pFile->namelist[TmpN]);

								//    if (BytesToSend == Len)
								//    { // buffer is filled up now exit

								//        pTxBuf->pFile->Length = 0;
								//        pTxBuf->pFile->File = (int)TmpN;
								//        Repeat = 0;
								//    }
								//}
								//else
								//{
								//    // No, now fill up to complete buffer size
								//    ULONG RemCnt;

								//    RemCnt = BytesToSend - Len;
								//    CommonHelper.memcpy((UBYTE*)(&(pTxBuf->Buf[Len])), (UBYTE*)TmpFileName, (int)RemCnt);
								//    Len += RemCnt;
								//    pTxBuf->pFile->Length = RemCnt;
								//    pTxBuf->pFile->File = (int)TmpN;
								//    Repeat = 0;
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}

                            // Update pointers
                            pTxBuf->pFile->Pointer += Len;
                            pTxBuf->SendBytes += Len;

							// TODO: some files shite
							//if (pTxBuf->pFile->Pointer == pTxBuf->pFile->Size)
							//{
							//    // Complete list has been tx'ed
							//    free(pTxBuf->pFile->namelist);
							//    cComFreeHandle((sbyte)pTxBuf->FileHandle);
							//}

							GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

							pTxBuf->Writing = 1;
                        }
                        break;

                    default:
                        {
                            // this is idle state
                        }
                        break;
                }
            }
        }


        public UBYTE cComFindMailbox(UBYTE* pName, UBYTE* pNo)
        {
            UBYTE RtnVal = 0;
            UBYTE Index;

            Index = 0;
            while ((0 != CommonHelper.strcmp((DATA8*)pName, (DATA8*)GH.ComInstance.MailBox[Index].Name)) && (Index < NO_OF_MAILBOXES))
            {
                Index++;
            }

            if (Index < NO_OF_MAILBOXES)
            {
                RtnVal = 1;
                *pNo = Index;
            }
            return (RtnVal);
        }


        //******* BYTE CODE SNIPPETS **************************************************


        /*! \page cCom Communication
         *  <hr size="1"/>
         *  <b>     opCOM_READY     </b>
         *
         *- Test if communication is busy                                     \n
         *- Dispatch status may be set to DSPSTAT.BUSYBREAK                           \n
         *- If name is 0 then own adapter status is evaluated                 \n
         *
         *  \param  (DATA8)  HARDWARE  - \ref transportlayers                 \n
         *  \param  (DATA8*) *pNAME    - Name of the remote/own device        \n
         *
         */
        /*! \brief  opCOM_READY byte code
         *
         */
        public void cComReady()
        {
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            DATA8 Hardware;
            DATA8* pName;
            IP TmpIp;
            UBYTE Status;
            UBYTE ChNos;
            UBYTE* ChNoArr = CommonHelper.Pointer1d<UBYTE>(NO_OF_BT_CHS);


            TmpIp = GH.Lms.GetObjectIp();
            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pName = (DATA8*)GH.Lms.PrimParPointer();

            if (0 == pName[0])
            {
                //Checking if own bt adapter is busy
                if (OK == GH.ComInstance.ComResult)
                {
                    Status = GH.Bt.cBtGetHciBusyFlag();
                }
                else
                {
                    Status = GH.ComInstance.ComResult;
                }

                if (BUSY == Status)
                {
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            else
            {
                ChNos = GH.Bt.cBtGetChNo((UBYTE*)pName, ChNoArr);
                if (ChNos != 0)
                {
                    if (1 == GH.ComInstance.TxBuf[ChNoArr[ChNos - 1] + BTSLAVE].Writing)
                    {
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                }
            }

            if (DspStat == DSPSTAT.BUSYBREAK)
            {
                // Rewind IP
                GH.Lms.SetObjectIp(TmpIp - 1);
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom Communication
         *  <hr size="1"/>
         *  <b>     opCOM_TEST     </b>
         *
         *- Test if communication is busy                                     \n
         *- Dispatch status is set to DSPSTAT.NOBREAK                                 \n
         *- If name is 0 then own adapter busy status is returned
         *
         *  \param  (DATA8)  HARDWARE  - \ref transportlayers                 \n
         *  \param  (DATA8*) *pNAME    - Name of the remote/own device        \n
         *  \return (DATA8)  Busy      - busy flag (0 = Ready, 1 = Busy)      \n
         *
         */
        /*! \brief  opCOM_TEST byte code
         *
         */
        public void cComTest()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 Busy = 0;
            DATA8 Hardware;
            DATA8* pName;
            UBYTE Status;
            UBYTE ChNos;
            UBYTE* ChNoArr = CommonHelper.Pointer1d<UBYTE>(NO_OF_BT_CHS);

            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pName = (DATA8*)GH.Lms.PrimParPointer();

            if (0 == pName[0])
            {
                //Checking if own bt adapter is busy
                if (OK == GH.ComInstance.ComResult)
                {
                    Status = GH.Bt.cBtGetHciBusyFlag();
                }
                else
                {
                    Status = GH.ComInstance.ComResult;
                }
                if (BUSY == Status)
                {
                    Busy = 1;
                }
            }
            else
            {
                ChNos = GH.Bt.cBtGetChNo((UBYTE*)pName, ChNoArr);
                if (ChNos != 0)
                {
                    if (1 == GH.ComInstance.TxBuf[ChNoArr[ChNos - 1] + BTSLAVE].Writing)
                    {
                        Busy = 1;
                    }
                }
            }

            *(DATA8*)GH.Lms.PrimParPointer() = Busy;
            DspStat = DSPSTAT.NOBREAK;
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom Communication
         *  <hr size="1"/>
         *  <b>     opCOM_READ (CMD, ....)  </b>
         *
         *- Communication read                                                \n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   CMD     - Specific command \ref comreadsubcode  \n
         *
         *  - CMD = COMMAND
         *  \param  (DATA32)  LENGTH   - Maximal code stream length           \n
         *  \return (DATA32)  *IMAGE   - Address of image                     \n
         *  \return (DATA32)  *GLOBAL  - Address of global variables          \n
         *  \return (DATA8)   FLAG     - Flag that tells if image is ready    \n
         *
         */
        /*! \brief  opCOM_READ byte code
         *
         */
        public void cComRead()
        {
            DATA8 Cmd;
            DATA32 pImage;
            DATA32 pGlobal;
            DATA8 Flag;

            if (GH.ComInstance.Cmdfd >= 0)
            {
                // Moved To lms2012.c
                //cComUpdate();
            }

            Cmd = *(DATA8*)GH.Lms.PrimParPointer();

            switch (Cmd)
            { // Function

                case COMMAND:
                    {
                        // pImage used as temp var
                        pImage = *(DATA32*)GH.Lms.PrimParPointer();

                        pImage = (DATA32)GH.ComInstance.Image;
                        pGlobal = (DATA32)GH.ComInstance.Globals;

                        Flag = (sbyte)GH.ComInstance.CommandReady;
                        GH.ComInstance.CommandReady = 0;

                        *(DATA32*)GH.Lms.PrimParPointer() = pImage;
                        *(DATA32*)GH.Lms.PrimParPointer() = pGlobal;
                        *(DATA8*)GH.Lms.PrimParPointer() = Flag;

                    }
                    break;
            }
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opCOM_WRITE (CMD, ....)  </b>
         *
         *- Communication write\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   CMD     - Specific command \ref comwritesubcode  \n
         *
         *  - CMD = REPLY
         *  \return (DATA32)  *IMAGE   - Address of image                      \n
         *  \return (DATA32)  *GLOBAL  - Address of global variables           \n
         *
         */
        /*! \brief  opCOM_WRITE byte code
         *
         */
        public void cComWrite()
        {
            DATA8 Cmd;
            DATA8 Status;
            DATA32 pImage;
            DATA32 pGlobal;
            COMCMD* pComCmd;
            IMGHEAD* pImgHead;

            Cmd = *(DATA8*)GH.Lms.PrimParPointer();

            switch (Cmd)
            { // Function

                case REPLY:
                    {
                        GH.ComInstance.VmReady = 1;
                        pImage = *(DATA32*)GH.Lms.PrimParPointer();
                        pGlobal = *(DATA32*)GH.Lms.PrimParPointer();
                        Status = *(DATA8*)GH.Lms.PrimParPointer();
                        pImgHead = (IMGHEAD*)pImage;

                        if ((DIR_CMD_REPLY & GH.ComInstance.ReplyStatus) != 0)
                        {
                            pComCmd = (COMCMD*)GH.ComInstance.TxBuf[GH.ComInstance.ActiveComCh].Buf;
                            (*pComCmd).CmdSize += (CMDSIZE)(*pImgHead).GlobalBytes; // Has been pre filled with the default length

                            if (OK == Status)
                            {
                                (*pComCmd).Cmd = DIRECT_REPLY;
                            }
                            else
                            {
                                (*pComCmd).Cmd = DIRECT_REPLY_ERROR;
                            }

                            CommonHelper.memcpy((*pComCmd).PayLoad, (UBYTE*)pGlobal, (int)(*pImgHead).GlobalBytes);

                            GH.ComInstance.TxBuf[GH.ComInstance.ActiveComCh].Writing = 1;
                            GH.ComInstance.TxBuf[GH.ComInstance.ActiveComCh].BlockLen = (uint)((*pComCmd).CmdSize + sizeof(CMDSIZE));
                        }

                        //Clear ReplyStatus both for DIR_CMD_REPLY and DIR_CMD_NOREPLY
                        GH.ComInstance.ReplyStatus = 0;
                    }
                    break;
            }
        }


        public void cComReadData()
        {
            DATA8 Hardware;
            DATA8* pName;
            DATA8 Size;
            DATA8* pData;

            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pName = (DATA8*)GH.Lms.PrimParPointer();
            Size = *(DATA8*)GH.Lms.PrimParPointer();
            pData = (DATA8*)GH.Lms.PrimParPointer();
        }


        public void cComWriteData()
        {
            DATA8 Hardware;
            DATA8* pName;
            DATA8 Size;
            DATA8* pData;

            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pName = (DATA8*)GH.Lms.PrimParPointer();
            Size = *(DATA8*)GH.Lms.PrimParPointer();
            pData = (DATA8*)GH.Lms.PrimParPointer();
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_OPEN   </b>
         *
         *- Open a mail box on the brick                                                                        \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)    NO        - Reference ID for the mailbox. Maximum number of mailboxes is 30      \n
         *  \param  (DATA8)    BOXNAME   - Zero terminated string with the mailbox name                         \n
         *  \param  (DATA8)    TYPE      - Data type of the content of the mailbox \ref formats "TYPE enum"     \n
         *  \param  (DATA8)    FIFOSIZE  - Not used                                                             \n
         *  \param  (DATA8)    VALUES    - Number of values of the type (specified by TYPE).                    \n
         *
         *
         *  If data type DATA_S is selected then it requires that a zero terminated string is send.             \n
         *
         *  Maximum mailbox size is 250 bytes. I.e. if type is string (DATA_S) then there can only be 1 string  \n
         *  of maximum 250 bytes (incl. zero termination), or if array (DATA_A), then array size cannot be      \n
         *  larger than 250 bytes                                                                               \n
         */
        /*! \brief  opMAILBOX_OPEN byte code
         *
         */
        public void cComOpenMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 No;
            DATA8* pBoxName;
            DATA8 Type;
            DATA8 Values;
            DATA8 FifoSize;

            No = *(DATA8*)GH.Lms.PrimParPointer();
            pBoxName = (DATA8*)GH.Lms.PrimParPointer();
            Type = *(DATA8*)GH.Lms.PrimParPointer();
            FifoSize = *(DATA8*)GH.Lms.PrimParPointer();
            Values = *(DATA8*)GH.Lms.PrimParPointer();

            if (OK != GH.ComInstance.MailBox[No].Status)
            {
                CommonHelper.snprintf((DATA8*)(&(GH.ComInstance.MailBox[No].Name[0])), 50, CommonHelper.GetString((DATA8*)pBoxName));
                CommonHelper.memset((byte*)GH.ComInstance.MailBox[No].Content, 0, MAILBOX_CONTENT_SIZE);
                GH.ComInstance.MailBox[No].Type = (byte)Type;
                GH.ComInstance.MailBox[No].Status = OK;
                GH.ComInstance.MailBox[No].ReadCnt = 0;
                GH.ComInstance.MailBox[No].WriteCnt = 0;
                DspStat = DSPSTAT.NOBREAK;
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_WRITE   </b>
         *
         *- Write to mailbox in remote brick                                                      \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  If Brick name is left empty (0) then all connected devices will                       \n
         *  receive the mailbox message
         *
         *  \param  (DATA8)    BRICKNAME  - Zero terminated string name of the receiving brick    \n
         *  \param  (DATA8)    HARDWARE   - Transportation media                                  \n
         *  \param  (DATA8)    BOXNAME    - Zero terminated string name of the receiving mailbox  \n
         *  \param  (DATA8)    TYPE       - Data type of the values \ref formats "TYPE enum"      \n
         *  \param  (DATA8)    VALUES     - Number of values of the specified type to send        \n
         *
         *  If string type (DATA_S) data is to be transmitted then a zero terminated string is    \n
         *  expected.
         *
         *  If array type data (DATA_A) is to be transmitted then the number of bytes to be send  \n
         *  is equal to the array size
         *
         */
        /*! \brief  opMAILBOX_WRITE byte code
         *
         */
        public void cComWriteMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8* pBrickName;
            DATA8 Hardware;
            DATA8* pBoxName;
            DATA8 Type;
            DATA8 Values;
            DATA32* Payload = CommonHelper.Pointer1d<DATA32>((MAILBOX_CONTENT_SIZE / 4) + 1);
            UBYTE ChNos;
            UBYTE ComChNo;
            UBYTE* ChNoArr = CommonHelper.Pointer1d<UBYTE>(NO_OF_BT_CHS);
            UBYTE Cnt;
            UWORD PayloadSize = 0;

            WRITE_MAILBOX* pComMbx;
            WRITE_MAILBOX_PAYLOAD* pComMbxPayload;

            pBrickName = (DATA8*)GH.Lms.PrimParPointer();
            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pBoxName = (DATA8*)GH.Lms.PrimParPointer();
            Type = *(DATA8*)GH.Lms.PrimParPointer();
            Values = *(DATA8*)GH.Lms.PrimParPointer();

            // It is needed that all parameters are pop'ed off regardless
            for (Cnt = 0; Cnt < Values; Cnt++)
            {
                switch (Type)
                {
                    case DATA_8:
                        {
                            ((DATA8*)(Payload))[Cnt] = *(DATA8*)GH.Lms.PrimParPointer();
                            PayloadSize++;
                        }
                        break;
                    case DATA_16:
                        {
                            ((DATA16*)(Payload))[Cnt] = *(DATA16*)GH.Lms.PrimParPointer();
                            PayloadSize += 2;
                        }
                        break;
                    case DATA_32:
                        {
                            ((DATA32*)(Payload))[Cnt] = *(DATA32*)GH.Lms.PrimParPointer();
                            PayloadSize += 4;
                        }
                        break;
                    case DATA_F:
                        {
                            ((DATAF*)(Payload))[Cnt] = *(DATAF*)GH.Lms.PrimParPointer();
                            PayloadSize += 4;
                        }
                        break;
                    case DATA_S:
                        {
                            // Supports only one string
                            DATA8* pName;

                            pName = (DATA8*)GH.Lms.PrimParPointer();

                            PayloadSize = (ushort)CommonHelper.snprintf((DATA8*)&Payload[0], MAILBOX_CONTENT_SIZE, CommonHelper.GetString((DATA8*)pName));
                            PayloadSize++; // Include zero termination
                        }
                        break;
                    case DATA_A:
                        {
                            DESCR* pDescr = null;
                            PRGID TmpPrgId;
                            DATA32 Size;
                            void* pTmp;
                            HANDLER TmpHandle;

                            TmpPrgId = GH.Lms.CurrentProgramId();
                            TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();

                            if (OK == GH.Memory.cMemoryGetPointer(TmpPrgId, TmpHandle, &pTmp))
                            {
                                pDescr = (DESCR*)pTmp;
                                Size = (pDescr->Elements);
                            }
                            else
                            {
                                Size = 0;
                            }

                            if (MAILBOX_CONTENT_SIZE < Size)
                            {
                                //Truncate if larger than buffer size
                                Size = MAILBOX_CONTENT_SIZE;
                            }

                            CommonHelper.memcpy((UBYTE*)&Payload[0], (UBYTE*)(pDescr->pArray), Size);
                            PayloadSize = (ushort)Size;
                        }
                        break;
                }
            }

            ChNos = GH.Bt.cBtGetChNo((UBYTE*)pBrickName, ChNoArr);

            for (Cnt = 0; Cnt < ChNos; Cnt++)
            {
                ComChNo = (byte)(ChNoArr[Cnt] + BTSLAVE);                               // Ch nos offset from BT module

                // Valid channel found
                if ((0 == GH.ComInstance.TxBuf[ComChNo].Writing) && (TXIDLE == GH.ComInstance.TxBuf[ComChNo].State))
                {
                    // Buffer is empty
                    pComMbx = (WRITE_MAILBOX*)GH.ComInstance.TxBuf[ComChNo].Buf;

                    // First part of message
                    (*pComMbx).CmdSize = SIZEOF_WRITEMAILBOX - sizeof(CMDSIZE);
                    (*pComMbx).MsgCount = 1;
                    (*pComMbx).CmdType = SYSTEM_COMMAND_NO_REPLY;
                    (*pComMbx).Cmd = WRITEMAILBOX;
                    (*pComMbx).NameSize = (byte)(CommonHelper.strlen((DATA8*)pBoxName) + 1);
                    CommonHelper.snprintf((DATA8*)(*pComMbx).Name, (*pComMbx).NameSize, CommonHelper.GetString((DATA8*)pBoxName));

                    (*pComMbx).CmdSize += (*pComMbx).NameSize;

                    // Payload part of message
                    pComMbxPayload = (WRITE_MAILBOX_PAYLOAD*)&(GH.ComInstance.TxBuf[ComChNo].Buf[(*pComMbx).CmdSize + sizeof(CMDSIZE)]);
                    (*pComMbxPayload).SizeLsb = (UBYTE)(PayloadSize & 0x00FF);
                    (*pComMbxPayload).SizeMsb = (UBYTE)((PayloadSize >> 8) & 0x00FF);
                    CommonHelper.memcpy((*pComMbxPayload).Payload, (byte*)Payload, PayloadSize);
                    (*pComMbx).CmdSize += (ushort)(PayloadSize + SIZEOF_WRITETOMAILBOXPAYLOAD);

                    GH.ComInstance.TxBuf[ComChNo].BlockLen = (uint)((*pComMbx).CmdSize + sizeof(CMDSIZE));
                    GH.ComInstance.TxBuf[ComChNo].Writing = 1;
                }
            }

            DspStat = DSPSTAT.NOBREAK;
            GH.Lms.SetDispatchStatus(DspStat);
        }

        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_READ  </b>
         *
         *- Read data from mailbox specified by NO                                           \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)    NO         - Messagebox ID of the message box you want to read\n
         *  \param  (DATA16)   LENGTH     - Maximum bytes to be read                         \n
         *  \param  (DATA8)    VALUES     - Number of value to read                          \n
         *  \return (Type specified in open)  VALUE      - Data from the message box         \n
         *
         *  The type of Value is specified by mailbox open byte code.                        \n
         */
        /*! \brief  opMAILBOX_READ byte code
        *
        */
        public void cComReadMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 No;
            DATA8 Values;
            DATA16 Len;
            UBYTE Cnt;

            No = *(DATA8*)GH.Lms.PrimParPointer();
            Len = *(DATA16*)GH.Lms.PrimParPointer();
            Values = *(DATA8*)GH.Lms.PrimParPointer();

            if (OK == GH.ComInstance.MailBox[No].Status)
            {
                for (Cnt = 0; Cnt < Values; Cnt++)
                {
                    switch (GH.ComInstance.MailBox[No].Type)
                    {
                        case DATA_8:
                            {
                                *(DATA8*)GH.Lms.PrimParPointer() = ((DATA8*)(GH.ComInstance.MailBox[No].Content))[Cnt];
                            }
                            break;
                        case DATA_16:
                            {
                                *(DATA16*)GH.Lms.PrimParPointer() = ((DATA16*)(GH.ComInstance.MailBox[No].Content))[Cnt];
                            }
                            break;
                        case DATA_32:
                            {
                                *(DATA32*)GH.Lms.PrimParPointer() = ((DATA32*)(GH.ComInstance.MailBox[No].Content))[Cnt];
                            }
                            break;
                        case DATA_F:
                            {
                                *(DATAF*)GH.Lms.PrimParPointer() = ((DATAF*)(GH.ComInstance.MailBox[No].Content))[Cnt];
                            }
                            break;
                        case DATA_S:
                            {
                                // Supports only one string
                                DATA8* pData;
                                DATA32 Data32;

                                pData = (DATA8*)GH.Lms.PrimParPointer();

                                if (GH.VMInstance.Handle >= 0)
                                {

                                    Data32 = GH.ComInstance.MailBox[No].DataSize;
                                    if (Data32 > MIN_ARRAY_ELEMENTS)
                                    {
                                        pData = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Data32);
                                    }
                                }

                                if (pData != null)
                                {
                                    CommonHelper.snprintf((DATA8*)pData, MAILBOX_CONTENT_SIZE, CommonHelper.GetString((DATA8*)GH.ComInstance.MailBox[No].Content));
                                }
                            }
                            break;
                        case DATA_A:
                            {
                                HANDLER TmpHandle;
                                DATA8* pData = null;
                                DATA32 Data32;

                                TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
                                Data32 = GH.ComInstance.MailBox[No].DataSize;

                                if (Data32 > MIN_ARRAY_ELEMENTS)
                                {
                                    pData = (DATA8*)GH.Lms.VmMemoryResize(TmpHandle, Data32);
                                }

                                if (null != pData)
                                {
                                    CommonHelper.memcpy((byte*)pData, (UBYTE*)&((DATA8*)(GH.ComInstance.MailBox[No].Content))[Cnt], Data32);
                                }
                                GH.ComInstance.MailBox[No].DataSize = 0;
                            }
                            break;
                    }
                }

                if (GH.ComInstance.MailBox[No].WriteCnt != GH.ComInstance.MailBox[No].ReadCnt)
                {
                    GH.ComInstance.MailBox[No].ReadCnt++;
                }
            }

            DspStat = DSPSTAT.NOBREAK;
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_TEST  </b>
         *
         *- Tests if new message has been read                                              \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)    NO         - Reference ID mailbox number                     \n
         *  \return (DATA8)    BUSY       - If Busy = 1 then no new messages are received\n
         */
        /*! \brief  opMAILBOX_TEST byte code
        *
        */
        public void cComTestMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 Busy = 0;
            DATA8 No;

            No = *(DATA8*)GH.Lms.PrimParPointer();

            if (GH.ComInstance.MailBox[No].WriteCnt == GH.ComInstance.MailBox[No].ReadCnt)
            {
                Busy = 1;
            }

            *(DATA8*)GH.Lms.PrimParPointer() = Busy;

            DspStat = DSPSTAT.NOBREAK;
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_READY  </b>
         *
         *- Waiting from message to be read                            \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)    NO         - Reference ID mailbox number\n
         */
        /*! \brief  opMAILBOX_READY byte code
        *
        */
        public void cComReadyMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            DATA8 No;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            No = *(DATA8*)GH.Lms.PrimParPointer();

            if (GH.ComInstance.MailBox[No].WriteCnt == GH.ComInstance.MailBox[No].ReadCnt)
            {
                DspStat = DSPSTAT.BUSYBREAK; ;
            }

            if (DspStat == DSPSTAT.BUSYBREAK)
            { // Rewind IP

                GH.Lms.SetObjectIp(TmpIp - 1);
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opMAILBOX_CLOSE  </b>
         *
         *- Closes mailbox indicated by NO                                  \n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *    -  \param  (DATA8)    NO         - Reference ID mailbox number\n
         */
        /*! \brief  opMAILBOX_CLOSE byte code
        *
        */
        public void cComCloseMailBox()
        {
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            DATA8 No;

            No = *(DATA8*)GH.Lms.PrimParPointer();

            GH.ComInstance.MailBox[No].Status = (byte)RESULT.FAIL;
            GH.Lms.SetDispatchStatus(DspStat);
        }


        public void cComWriteFile()
        {
            TXBUF* pTxBuf;
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;

            DATA8 Hardware;
            DATA8* pDeviceName;
            DATA8* pFileName;
            DATA8 FileType;

            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pDeviceName = (DATA8*)GH.Lms.PrimParPointer();
            pFileName = (DATA8*)GH.Lms.PrimParPointer();
            FileType = *(DATA8*)GH.Lms.PrimParPointer();

            GH.printf($"{FileType} [{CommonHelper.GetString((DATA8*)pFileName)}] ->> {Hardware} [{CommonHelper.GetString((DATA8*)pDeviceName)}]\r\n");

            switch (Hardware)
            {
                case HW_USB:
                    {
                    }
                    break;

                case HW_BT:
                    {
                        UBYTE* ChNo = CommonHelper.Pointer1d<UBYTE>(NO_OF_BT_CHS);
                        UBYTE* pName = CommonHelper.Pointer1d<UBYTE>(MAX_FILENAME_SIZE);
                        BEGIN_DL* pDlMsg;

                        GH.Bt.cBtGetChNo((UBYTE*)pDeviceName, ChNo);

                        ChNo[0] += BTSLAVE;                           // Add Com module offset
                        pTxBuf = &(GH.ComInstance.TxBuf[ChNo[0]]);
                        pDlMsg = (BEGIN_DL*)(&(pTxBuf->Buf[0]));

                        if (TYPE_FOLDER == FileType)
                        {
                            // Sending folder
                            pTxBuf->State = TXFOLDER;

                            // Make copy of Foldername including a "/" at the end
                            CommonHelper.snprintf((DATA8*)pTxBuf->Folder, MAX_FILENAME_SIZE, CommonHelper.GetString(pFileName));
                            CommonHelper.strcat((DATA8*)pTxBuf->Folder, "/".AsSbytePointer());

                            pTxBuf->pDir = (DATA8*)pFileName;

                            if (cComGetNxtFile(pTxBuf->pDir, pName) != 0)
                            {
                                // File has been located
                                GH.ComInstance.ComResult = BUSY;
                                cComCreateBeginDl(pTxBuf, pName);
                            }
                            else
                            {
                                // No files in directory
                                GH.ComInstance.ComResult = OK;
                            }
                            DspStat = DSPSTAT.NOBREAK;
                        }
                        else
                        {
                            // Sending single file
                            pTxBuf->State = TXFILE;
                            pTxBuf->Folder[0] = 0;           // Make sure that folder is empty
                            GH.ComInstance.ComResult = BUSY;
                            cComCreateBeginDl(pTxBuf, (UBYTE*)pFileName);
                            DspStat = DSPSTAT.NOBREAK;
                        }
                    }
                    break;

                case HW_WIFI:
                    {
                    }
                    break;
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        //static    DATA8 BtParred      =  0;

        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opCOM_GET (CMD, ....)  </b>
         *
         *- Communication get entry\n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)   CMD               - \ref comgetsetsubcode
         *
         *\n
         *  - CMD = GET_ON_OFF
         *\n  Get active state                                                         \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \return (DATA8)    ACTIVE      - Active [0,1]                         \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_VISIBLE
         *\n  Get visibility state                                                     \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \return (DATA8)    VISIBLE     - Visible [0,1]                        \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_RESULT
         *\n  Get status. This command gets the result of the command that             \n
         *    is being executed. This could be a search or a connection                \n
         *    request.                                                                 \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Name index                           \n
         *    -  \return (DATA8)    RESULT      - \ref results                         \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_PIN
         *\n  Get pin code. For now "1234" is returned                                 \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    NAME        - First character in character string  \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    PINCODE     - First character in character string  \n
         *
         *\n
         *
         *\n
         *  - CMD = SEARCH_ITEMS
         *\n  Get number of item from search. After a search has been completed,       \n
         *    SEARCH ITEMS will return the number of remote devices found.             \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \return (DATA8)    ITEMS       - No of items in seach list            \n
         *
         *\n
         *
         *\n
         *  - CMD = SEARCH_ITEM
         *\n  Get search item informations. Used to retrieve the item information      \n
         *    in the search list                                                       \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Item - index in search list          \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    NAME        - First character in character string  \n
         *    -  \return (DATA8)    PARRED      - Parred [0,1]                         \n
         *    -  \return (DATA8)    CONNECTED   - Connected [0,1]                      \n
         *    -  \return (DATA8)    TYPE        - \ref bttypes                         \n
         *    -  \return (DATA8)    VISIBLE     - Visible [0,1]                        \n
         *
         *\n
         *
         *\n
         *  - CMD = FAVOUR_ITEMS
         *\n  Get no of item in favourite list. The number of paired devices, not      \n
         *    necessarily visible or present devices                                   \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \return (DATA8)    ITEMS       - No of items in list                  \n
         *
         *\n
         *
         *\n
         *  - CMD = FAVOUR_ITEM
         *\n  Get favourite item informations. Used to retrieve the item information   \n
         *    in the favourite list. All items in the favourite list are paired devices\n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Item - index in favourite list       \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    NAME        - First character in character string  \n
         *    -  \return (DATA8)    PARRED      - Parred [0,1]                         \n
         *    -  \return (DATA8)    CONNECTED   - Connected [0,1]                      \n
         *    -  \return (DATA8)    TYPE        - \ref bttypes                         \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_ID
         *\n  Get bluetooth address information                                        \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    STRING      - First character in BT adr string     \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_BRICKNAME
         *\n  Gets the name of the brick                                               \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    NAME        - First character in brick name        \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_NETWORK
         *\n  Gets the network information. WIFI only                                  \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \return (DATA8)    NAME        - First character in AP name           \n
         *    -  \return (DATA8)    MAC         - First character in MAC adr string    \n
         *    -  \return (DATA8)    IP          - First character in IP no string      \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_PRESENT
         *\n  Return if hardare is present. WIFI only                                  \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \return (DATA8)    OK          - Present [0,1]                        \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_ENCRYPT
         *\n  Returns the encryption mode of the hardware. WIFI only                   \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Item - index in favourite list       \n
         *    -  \param  (DATA8)    TYPE        - Encryption type                      \n
         *
         *\n
         *
         *\n
         *  - CMD = GET_INCOMMING
         *\n  Returns the encryption mode of the hardware. WIFI only                   \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    LENGTH      - Max length of returned string        \n
         *    -  \param  (DATA8)    NAME        - First character in name              \n
         *
         *\n
         *
         */
        /*! \brief  opCOM_GET byte code
         *
         */


        public void cComGet()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 Cmd;
            DATA8 Hardware;
            DATA8 OnOff;
            DATA8 Mode2;
            DATA8 Item;
            DATA8 Status;
            DATA8* pName;
            DATA8 Length;
            DATA8* pPin;
            DATA8 Items;
            DATA8 Parred = 0;
            DATA8 Connected = 0;
            DATA8 Visible = 0;
            DATA8 Type = 0;
            DATA8* pMac;
            DATA8* pIp;

            Cmd = *(DATA8*)GH.Lms.PrimParPointer();

            switch (Cmd)
            { // Function

                case GET_MODE2:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Mode2 = 0;

                        switch (Hardware)
                        {
                            case HW_BT:
                                {

                                    if (RESULT.FAIL != (RESULT)GH.Bt.BtGetMode2((UBYTE*)&Mode2))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Mode2;
                    }
                    break;

                case GET_ON_OFF:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        OnOff = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (RESULT.FAIL != (RESULT)GH.Bt.BtGetOnOff((UBYTE*)&OnOff))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (OK == GH.Wifi.cWiFiGetOnStatus())
                                    {
                                        OnOff = 1;
                                    }
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }

                        *(DATA8*)GH.Lms.PrimParPointer() = OnOff;
                    }
                    break;

                case GET_VISIBLE:
                    {
                        DATA8 VisibleLocal = 0;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();

                        if (Hardware < HWTYPES)
                        {
							// Fill in code here
							VisibleLocal = (sbyte)GH.Bt.BtGetVisibility();

                            DspStat = DSPSTAT.NOBREAK;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = VisibleLocal;
                    }
                    break;

                case GET_RESULT:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();
                        Status = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (OK == GH.ComInstance.ComResult)
                                    {
                                        Status = (sbyte)GH.Bt.cBtGetHciBusyFlag();
                                    }
                                    else
                                    {
                                        Status = (sbyte)GH.ComInstance.ComResult;
                                    }
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    Status = (sbyte)GH.Wifi.cWiFiGetStatus();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }

                        *(DATA8*)GH.Lms.PrimParPointer() = Status;
                    }
                    break;

                case GET_PIN:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pPin = (DATA8*)GH.Lms.PrimParPointer();

                        // Fill in code here
                        CommonHelper.snprintf((DATA8*)pPin, Length, "%s", "1234");

                        DspStat = DSPSTAT.NOBREAK;

                    }
                    break;

                case CONNEC_ITEMS:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Items = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_BT:
                                {
                                    Items = (sbyte)GH.Bt.cBtGetNoOfConnListEntries();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Items;
                    }
                    break;

                case CONNEC_ITEM:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = vmBRICKNAMESIZE;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_BT:
                                {
                                    if (null != pName)
                                    {
										GH.Bt.cBtGetConnListEntry((byte)Item, (UBYTE*)pName, Length, (UBYTE*)&Type);
                                        *(DATA8*)GH.Lms.PrimParPointer() = Type;
                                    }
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                    }
                    break;

                case SEARCH_ITEMS:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Items = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    Items = (sbyte)GH.Bt.cBtGetNoOfSearchListEntries();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    Items = (sbyte)GH.Wifi.cWiFiGetApListSize();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Items;
                    }
                    break;

                case SEARCH_ITEM:
                    {
                        UBYTE Flags;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = vmBRICKNAMESIZE;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    Parred = 0;
                                    Connected = 0;
                                    Type = ICON_UNKNOWN;
                                    Visible = 1;

                                    if (null != pName)
                                    {
										GH.Bt.cBtGetSearchListEntry((byte)Item, &Connected, &Type, &Parred, (UBYTE*)pName, Length);
                                    }

                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    Parred = 0;
                                    Connected = 0;
                                    Type = 0;
                                    Visible = 0;

                                    if (null != pName)
                                    {
										GH.Wifi.cWiFiGetName((DATA8*)pName, (int)Item, Length);
                                        Flags = GH.Wifi.cWiFiGetFlags(Item);

                                        if ((Flags & VISIBLE) != 0)
                                        {
                                            Visible = 1;
                                        }
                                        if ((Flags & CONNECTED) != 0)
                                        {
                                            Connected = 1;
                                        }
                                        if ((Flags & KNOWN) != 0)
                                        {
                                            Parred = 1;
                                        }
                                        if ((Flags & WPA2) != 0)
                                        {
                                            Type = 1;
                                        }
                                    }

                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Parred;
                        *(DATA8*)GH.Lms.PrimParPointer() = Connected;
                        *(DATA8*)GH.Lms.PrimParPointer() = Type;
                        *(DATA8*)GH.Lms.PrimParPointer() = Visible;
                    }
                    break;

                case FAVOUR_ITEMS:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Items = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    Items = (sbyte)GH.Bt.cBtGetNoOfDevListEntries();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Items;
                    }
                    break;

                case FAVOUR_ITEM:
                    {
                        UBYTE Flags;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = vmBRICKNAMESIZE;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    Parred = 1;                      // Only in favour list if parred
                                    Connected = 0;
                                    Type = ICON_UNKNOWN;

                                    if (null != pName)
                                    {
                                        GH.Bt.cBtGetDevListEntry((byte)Item, &Connected, &Type, (UBYTE*)pName, Length);
                                    }
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    Parred = 0;                      // Only in favour list if parred
                                    Connected = 0;
                                    Type = 0;

                                    if (null != pName)
                                    {
                                        GH.Wifi.cWiFiGetName((DATA8*)pName, (int)Item, Length);
                                        Flags = GH.Wifi.cWiFiGetFlags(Item);

                                        if ((Flags & CONNECTED) != 0)
                                        {
                                            Connected = 1;
                                        }
                                        if ((Flags & KNOWN) != 0)
                                        {
                                            Parred = 1;
                                        }
                                        if ((Flags & WPA2) != 0)
                                        {
                                            Type = 1;
                                        }
                                    }

                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }

                        *(DATA8*)GH.Lms.PrimParPointer() = Parred;
                        *(DATA8*)GH.Lms.PrimParPointer() = Connected;
                        *(DATA8*)GH.Lms.PrimParPointer() = Type;
                    }
                    break;

                case GET_ID:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (GH.VMInstance.Handle >= 0)
                                    {
                                        if (-1 == Length)
                                        {
                                            Length = vmBTADRSIZE;
                                        }
                                        pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                                    }

                                    if (pName != null)
                                    {
                                        GH.Bt.cBtGetId((UBYTE*)pName, (byte)Length);
                                    }
                                    DspStat = DSPSTAT.NOBREAK;

                                }
                                break;
                            case HW_WIFI:
                                {
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }

                    }
                    break;

                case GET_BRICKNAME:
                    {
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = vmBRICKNAMESIZE;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        if (null != pName)
                        {
                            CommonHelper.snprintf((DATA8*)pName, Length, CommonHelper.GetString((DATA8*)&(GH.ComInstance.BrickName[0])));
                        }

                        DspStat = DSPSTAT.NOBREAK;
                    }
                    break;

                case GET_NETWORK:
                    {
                        UBYTE Flags;
                        UBYTE MaxStrLen;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();

                        // Lenght are the maximum length for all
                        // 3 strings returned (pName, pMac and pIp)
                        if ((vmBRICKNAMESIZE >= vmMACSIZE) && (vmBRICKNAMESIZE >= vmIPSIZE))
                        {
                            MaxStrLen = vmBRICKNAMESIZE;
                        }
                        else
                        {
                            if (vmMACSIZE >= vmIPSIZE)
                            {
                                MaxStrLen = vmMACSIZE;
                            }
                            else
                            {
                                MaxStrLen = vmIPSIZE;
                            }
                        }

                        pName = (DATA8*)GH.Lms.PrimParPointer();
                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = (sbyte)MaxStrLen;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        pMac = (DATA8*)GH.Lms.PrimParPointer();
                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = (sbyte)MaxStrLen;
                            }
                            pMac = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        pIp = (DATA8*)GH.Lms.PrimParPointer();
                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = (sbyte)MaxStrLen;
                            }
                            pIp = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if ((null != pName) && (null != pMac) && (null != pIp))
                                    {
                                        if (OK == GH.Wifi.cWiFiGetOnStatus())
                                        {
                                            Flags = GH.Wifi.cWiFiGetFlags((int)0);
                                            if ((Flags & CONNECTED) != 0)
                                            {
												GH.Wifi.cWiFiGetIpAddr((DATA8*)pIp);
												GH.Wifi.cWiFiGetMyMacAddr((DATA8*)pMac);
												GH.Wifi.cWiFiGetName((DATA8*)pName, 0, Length);
                                            }
                                            else
                                            {
												GH.Wifi.cWiFiGetMyMacAddr((DATA8*)pMac);
                                                CommonHelper.snprintf((DATA8*)pName, Length, "%s", "NONE");
                                                CommonHelper.snprintf((DATA8*)pIp, 3, "%s", "??");
                                            }
                                        }
                                        else
                                        {
                                            CommonHelper.snprintf((DATA8*)pMac, 3, "%s", "??");
                                            CommonHelper.snprintf((DATA8*)pName, Length, "%s", "NONE");
                                            CommonHelper.snprintf((DATA8*)pIp, 3, "%s", "??");
                                        }
                                    }
                                }
                                break;
                        }

                        DspStat = DSPSTAT.NOBREAK;
                    }
                    break;

                case GET_PRESENT:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Status = 0;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                    Status = 1;
                                }
                                break;
                            case HW_BT:
                                {
                                    Status = 1;
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (OK == GH.Wifi.cWiFiKnownDongleAttached())
                                    {
                                        Status = 1;
                                    }
                                }
                                break;
                        }
                        *(DATA8*)GH.Lms.PrimParPointer() = Status;
                        DspStat = DSPSTAT.NOBREAK;
                    }
                    break;

                case GET_ENCRYPT:
                    {

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();

                        Type = 0;
                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }

                        *(DATA8*)GH.Lms.PrimParPointer() = Type;
                    }
                    break;

                case GET_INCOMING:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Length = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        if (GH.VMInstance.Handle >= 0)
                        {
                            if (-1 == Length)
                            {
                                Length = vmBRICKNAMESIZE;
                            }
                            pName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
                        }

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (null != pName)
                                    {
										GH.Bt.cBtGetIncoming((UBYTE*)pName, (UBYTE*)&Type, (byte)Length);
                                    }

                                    *(DATA8*)GH.Lms.PrimParPointer() = Type;
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                            case HW_WIFI:
                                {
                                }
                                break;
                        }
                    }
                    break;

            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cCom
         *  <hr size="1"/>
         *  <b>     opCOM_SET (CMD, ....)  </b>
         *
         *- Communication set entry\n
         *- Dispatch status can return DSPSTAT.FAILBREAK
         *
         *  \param  (DATA8)   CMD               - \ref comgetsetsubcode
         *
         *
         *\n
         *  - CMD = SET_MODE2
         *\n  Set active mode state, either active or not\n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ACTIVE      - Active [0,1], 1 = on, 0 = off        \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_ON_OFF
         *\n  Set active state, either on or off\n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ACTIVE      - Active [0,1], 1 = on, 0 = off        \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_VISIBLE
         *\n  Set visibility state - Only available for bluetooth                      \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    VISIBLE     - Visible [0,1], 1 = visible, 0 = invisible\n
         *
         *\n
         *
         *\n
         *  - CMD = SET_SEARCH
         *\n  Control search. Starts or or stops the search for remote devices         \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    SEARCH      - Search [0,1] 0 = stop search, 1 = start search\n
         *
         *\n
         *
         *\n
         *  - CMD = SET_PIN
         *\n  Set pin code. Set the pincode for a remote device.                       \n
         *    Used when requested by bluetooth.                                        \n
         *    not at this point possible by user program                               \n
         *    -  \param  *(DATA8)   HARDWARE    - \ref transportlayers                 \n
         *    -  \param   (DATA8)   NAME        - First character in character string  \n
         *    -  \param   (DATA8)   PINCODE     - First character in character string  \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_PASSKEY
         *\n  Set pin code. Set the pincode for a remote device.                       \n
         *    Used when requested by bluetooth.                                        \n
         *    not at this point possible by user program                               \n
         *    -  \param  *(DATA8)   Hardware    - \ref transportlayers                 \n
         *    -  \param  *(DATA8)   ACCEPT      - Acceptance [0,1] 0 = reject 1 = accept \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_CONNECTION
         *\n  Control connection. Initiate or closes the connection request to a       \n
         *    remote device by the specified name                                      \n
         *    -  \param  (DATA8)    Hardware    - \ref transportlayers                 \n
         *    -  \param *(DATA8)    NAME        - First character in Name              \n
         *    -  \param  (DATA8)    CONNECTION  - Connect [0,1], 1 = Connect, 0 = Disconnect\n
         *
         *\n
         *
         *\n
         *  - CMD = SET_BRICKNAME
         *\n  Sets the name of the brick\n
         *    -  \param  (DATA8)    NAME        - First character in character string  \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_MOVEUP
         *\n  Moves the index in list one step up. Used to re-arrange WIFI list        \n
         *    Only used for WIFI                                                       \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Index in table                       \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_MOVEDOWN
         *\n  Moves the index in list one step down. Used to re-arrange WIFI list      \n
         *    Only used for WIFI                                                       \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Index in table                       \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_ENCRYPT
         *\n  Moves the index in list one step down. Only used for WIFI                \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param  (DATA8)    ITEM        - Index in table                       \n
         *    -  \param  (DATA8)    ENCRYPT     - Encryption type                      \n
         *
         *\n
         *
         *\n
         *  - CMD = SET_SSID
         *\n  Sets the SSID name. Only used for WIFI                                   \n
         *    -  \param  (DATA8)    HARDWARE    - \ref transportlayers                 \n
         *    -  \param *(DATA8)    NAME        - Index in table                       \n
         *
         *\n
         *
         */
        /*! \brief  opCOM_SET byte code
         *
         */
        public void cComSet()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 Cmd;
            DATA8 Hardware;
            DATA8 OnOff;
            DATA8 Mode2;
            DATA8 Visible;
            DATA8 Search;
            DATA8* pName;
            DATA8* pPin;
            DATA8 Connection;
            DATA8 Item;
            DATA8 Type;

            Cmd = *(DATA8*)GH.Lms.PrimParPointer();

            switch (Cmd)
            { // Function
                case SET_MODE2:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Mode2 = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_BT:
                                {
                                    if (RESULT.FAIL != (RESULT)GH.Bt.BtSetMode2((byte)Mode2))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                    }
                    break;

                case SET_ON_OFF:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        OnOff = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (RESULT.FAIL != (RESULT)GH.Bt.BtSetOnOff((byte)OnOff))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (OnOff != 0)
                                    {
                                        if (RESULT.FAIL != GH.Wifi.cWiFiTurnOn())
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                    else
                                    {
                                        if (RESULT.FAIL != GH.Wifi.cWiFiTurnOff())
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                    }
                    break;

                case SET_VISIBLE:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Visible = *(DATA8*)GH.Lms.PrimParPointer();
                        DspStat = DSPSTAT.NOBREAK;

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    // Fill in code here
                                    if (RESULT.FAIL != (RESULT)GH.Bt.BtSetVisibility((byte)Visible))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    // N_A
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                    }
                    break;

                case SET_SEARCH:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Search = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (Search != 0)
                                    {
                                        if (RESULT.FAIL != (RESULT)GH.Bt.BtStartScan())
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                    else
                                    {
                                        if (RESULT.FAIL != (RESULT)GH.Bt.BtStopScan())
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (Search != 0)
                                    {
                                        if (RESULT.FAIL != GH.Wifi.cWiFiScanForAPs())
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                    else
                                    {
                                        //NA in Wifi
                                    }
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                    }
                    break;

                case SET_PIN:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();
                        pPin = (DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (RESULT.FAIL != (RESULT)GH.Bt.cBtSetPin((UBYTE*)pPin))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    GH.printf($"\n\rSSID = {CommonHelper.GetString(pName)}\r\n");
                                    if (OK == GH.Wifi.cWiFiGetIndexFromName((DATA8*)pName, (UBYTE*)&Item))
                                    {
                                        GH.printf($"\r\nGot Index = {Item}\r\n");
                                        GH.printf($"\r\nGot Index from name = {CommonHelper.GetString(pName)}\r\n");
										GH.Wifi.cWiFiMakePsk((DATA8*)pName, (DATA8*)pPin, (int)Item);
                                        GH.printf("\r\nPSK made\r\n");
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case SET_PASSKEY:
                    {
                        UBYTE Accept;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Accept = *(UBYTE*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_BT:
                                {
                                    if (RESULT.FAIL != (RESULT)GH.Bt.cBtSetPasskey(Accept))
                                    {
                                        DspStat = DSPSTAT.NOBREAK;
                                    }
                                }
                                break;
                            default:
                                {
                                    //Only used in bluetooth
                                }
                                break;
                        }
                    }
                    break;

                case SET_CONNECTION:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();
                        Connection = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                    if (Connection != 0)
                                    {
                                        if (RESULT.FAIL != (RESULT)GH.Bt.cBtConnect((UBYTE*)pName))
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }

                                    }
                                    else
                                    {
                                        if (RESULT.FAIL != (RESULT)GH.Bt.cBtDisconnect((UBYTE*)pName))
                                        {
                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                    }
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (Connection != 0)
                                    {
                                        if (OK == GH.Wifi.cWiFiGetIndexFromName((DATA8*)pName, (UBYTE*)&Item))
                                        {
                                            GH.printf($"cWiFiConnect => index: {Item}, Name: {CommonHelper.GetString(pName)}\n\r");

											GH.Wifi.cWiFiConnectToAp((int)Item);

                                            GH.printf("We have tried to connect....\n\r");

                                            DspStat = DSPSTAT.NOBREAK;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {
                                        // Not implemented
                                        // implicite in connect
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case SET_BRICKNAME:
                    {
                        UBYTE Len;
                        DATA8* nl = CommonHelper.Pointer1d<DATA8>(1);

                        nl[0] = 0x0A;                                 // Insert new line

                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        Len = (byte)CommonHelper.strlen((DATA8*)pName);

                        if (OK == GH.Lms.ValidateString(pName, vmCHARSET_NAME) && ((vmBRICKNAMESIZE - 1) > CommonHelper.strlen((DATA8*)pName)))
                        {
                            if (RESULT.FAIL != (RESULT)GH.Bt.cBtSetName((UBYTE*)pName, (byte)(Len + 1)))
                            {
                                using FileStream fs = File.OpenWrite("./Resources/other/BrickName");
                                if (fs != null)
                                {
                                    fs.Write(CommonHelper.GetArray((byte*)pName, Len + 1));
                                    fs.WriteByte((byte)nl[0]);

                                    fs.Close();
                                }
                                CommonHelper.snprintf((DATA8*)&(GH.ComInstance.BrickName[0]), vmBRICKNAMESIZE, CommonHelper.GetString((DATA8*)pName));
                                // sethostname((DATA8*)pName, Len + 1); // TODO: wtf
                                DspStat = DSPSTAT.NOBREAK;
                            }
                        }
                    }
                    break;

                case SET_MOVEUP:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
                                    GH.Wifi.cWiFiMoveUpInList((int)Item);
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                    }
                    break;

                case SET_MOVEDOWN:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        Item = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
									GH.Wifi.cWiFiMoveDownInList((int)Item);
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                    }
                    break;

                case SET_ENCRYPT:
                    {
                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();
                        Type = *(DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
                                    if (Type != 0)
                                    {
                                        GH.printf("\r\nWPA encrypt called\r\n");

                                        uint LocalIndex = 0;
										GH.Wifi.cWiFiGetIndexFromName((DATA8*)pName, (UBYTE*)&LocalIndex);
										GH.Wifi.cWiFiSetEncryptToWpa2((int)LocalIndex);
                                    }
                                    else
                                    {
                                        GH.printf("\r\nNONE encrypt called\r\n");

										GH.Wifi.cWiFiSetEncryptToNone(GH.Wifi.cWiFiGetApListSize());
                                    }
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                    }
                    break;

                case SET_SSID:
                    {
                        UBYTE Index;

                        Hardware = *(DATA8*)GH.Lms.PrimParPointer();
                        pName = (DATA8*)GH.Lms.PrimParPointer();

                        switch (Hardware)
                        {
                            case HW_USB:
                                {
                                }
                                break;
                            case HW_BT:
                                {
                                }
                                break;
                            case HW_WIFI:
                                {
                                    Index = (byte)GH.Wifi.cWiFiGetApListSize();
									GH.Wifi.cWiFiSetName((DATA8*)pName, (int)Index);
									GH.Wifi.cWiFiIncApListSize();
                                    DspStat = DSPSTAT.NOBREAK;
                                }
                                break;
                        }
                    }
                    break;

            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        public void cComRemove()
        {
            DSPSTAT DspStat = DSPSTAT.FAILBREAK;
            DATA8 Hardware;
            DATA8* pName;
            UBYTE LocalIndex;

            Hardware = *(DATA8*)GH.Lms.PrimParPointer();
            pName = (DATA8*)GH.Lms.PrimParPointer();

            switch (Hardware)
            {
                case HW_USB:
                    {
                    }
                    break;
                case HW_BT:
                    {
                        if (RESULT.FAIL != (RESULT)GH.Bt.cBtDeleteFavourItem((UBYTE*)pName))
                        {
                            DspStat = DSPSTAT.NOBREAK;
                        }
                    }
                    break;
                case HW_WIFI:
                    {
						GH.Wifi.cWiFiGetIndexFromName((DATA8*)pName, (UBYTE*)&LocalIndex);

                        GH.printf($"Removing Index: {LocalIndex}\n\r");

						GH.Wifi.cWiFiDeleteAsKnown(LocalIndex);       // We removes the (favorit) "*"
                        DspStat = DSPSTAT.NOBREAK;
                    }
                    break;
            }
        }

        public UBYTE cComGetBtStatus()
        {
            return (GH.Bt.cBtGetStatus());
        }


        public UBYTE cComGetWifiStatus()
        {
            UBYTE Flags;
            UBYTE Status;

            Status = 0;

            if (OK == GH.Wifi.cWiFiGetOnStatus())
            {
                Status |= 0x03;                 // Wifi on + visible (always visible if on)
                Flags = GH.Wifi.cWiFiGetFlags((int)0);
                if ((CONNECTED & Flags) != 0)
                {
                    Status |= 0x04;                // Wifi connected to AP
                }
            }
            return (Status);
        }


        public void cComGetBrickName(DATA8 Length, DATA8* pBrickName)
        {
            CommonHelper.snprintf((DATA8*)pBrickName, Length, CommonHelper.GetString((DATA8*)&(GH.ComInstance.BrickName[0])));
        }


        public DATA8 cComGetEvent()
        {
            return ((sbyte)GH.Bt.cBtGetEvent());
        }

    }
}
