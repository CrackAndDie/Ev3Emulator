using Ev3Core.Ccom.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Extensions;
using static Ev3Core.Defines;

namespace Ev3Core.Ccom
{
    public class Com_ : ICom
    {
        public RESULT cComInit()
        {
            RESULT Result = OK;
            UWORD TmpFileHandle;
            UBYTE Cnt;

            GH.ComInstance.CommandReady = 0;

            for (TmpFileHandle = 0; TmpFileHandle < MAX_FILE_HANDLES; TmpFileHandle++)
            {
                GH.ComInstance.Files[TmpFileHandle].State = FS_IDLE;
                GH.ComInstance.Files[TmpFileHandle].Name[0] = (char)0;
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
            GH.ComInstance.ReadChannel[2] = null;
            GH.ComInstance.ReadChannel[3] = null;
            GH.ComInstance.ReadChannel[4] = null;
            GH.ComInstance.ReadChannel[5] = null;
            GH.ComInstance.ReadChannel[6] = null;
            GH.ComInstance.ReadChannel[7] = null;
            GH.ComInstance.ReadChannel[8] = null;
            GH.ComInstance.ReadChannel[9] = null;
            GH.ComInstance.ReadChannel[10] = GH.Wifi.cWiFiReadTcp;

            GH.ComInstance.WriteChannel[0] = cComWriteBuffer;
            GH.ComInstance.WriteChannel[1] = null;
            GH.ComInstance.WriteChannel[2] = null;
            GH.ComInstance.WriteChannel[3] = null;
            GH.ComInstance.WriteChannel[4] = null;
            GH.ComInstance.WriteChannel[5] = null;
            GH.ComInstance.WriteChannel[6] = null;
            GH.ComInstance.WriteChannel[7] = null;
            GH.ComInstance.WriteChannel[8] = null;
            GH.ComInstance.WriteChannel[9] = null;
            GH.ComInstance.WriteChannel[10] = GH.Wifi.cWiFiWriteTcp;

            for (Cnt = 0; Cnt < NO_OF_MAILBOXES; Cnt++)
            {
                GH.ComInstance.MailBox[Cnt].Status = FAIL;
                GH.ComInstance.MailBox[Cnt].DataSize = 0;
                GH.ComInstance.MailBox[Cnt].ReadCnt = 0;
                GH.ComInstance.MailBox[Cnt].WriteCnt = 0;
                GH.ComInstance.MailBox[Cnt].Name[0] = 0;
            }

            GH.ComInstance.ComResult = OK;

            GH.ComInstance.BrickName = "test".ToByteArray();

            GH.Wifi.cWiFiInit();

            GH.ComInstance.VmReady = 1;
            GH.ComInstance.ReplyStatus = 0;

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
                GH.ComInstance.MailBox[Cnt].Status = FAIL;
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

            Result = OK;

            GH.Wifi.cWiFiExit();

            return (Result);
        }

        UWORD cComReadBuffer(UBYTE[] pBuffer, UWORD Size)
        {
            UWORD Length = 0;

            // TODO: wtf was this
            return (Length);
        }

        UWORD cComWriteBuffer(UBYTE[] pBuffer, UWORD Size)
        {
            UWORD Length = 0;

            // TODO: wtf was this
            return (Length);
        }

        UBYTE cComDirectCommand(UBYTE[] pBuffer, UBYTE[] pReply)
        {
            UBYTE Result = 0;
            COMCMD pComCmd;
            COMRPL pComRpl;
            DIRCMD pDirCmd;
            CMDSIZE CmdSize;
            CMDSIZE HeadSize;
            UWORD Tmp;
            UWORD Globals;
            UWORD Locals;
            IMGHEAD pImgHead;
            OBJHEAD pObjHead;
            CMDSIZE Length;

            // TODO: DIRECT COMMAND anime
            GH.ComInstance.VmReady = 0;
            //pComCmd = (COMCMD*)pBuffer;
            //pDirCmd = (DIRCMD*)pComCmd.PayLoad;

            //CmdSize = pComCmd.CmdSize;
            //HeadSize = (pDirCmd.Code - pBuffer) - sizeof(CMDSIZE);
            //Length = (ushort)(CmdSize - HeadSize);

            //pComRpl = (COMRPL*)pReply;
            //pComRpl.CmdSize = 3;
            //pComRpl.MsgCnt = pComCmd.MsgCnt;
            //pComRpl.Cmd = DIRECT_REPLY_ERROR;

            //if ((CmdSize > HeadSize) && ((CmdSize - HeadSize) < (sizeof(ComInstance.Image) - (sizeof(IMGHEAD) + sizeof(OBJHEAD)))))
            //{

            //    Tmp = (UWORD)(*pDirCmd).Globals + ((UWORD)(*pDirCmd).Locals << 8);

            //    Globals = ((ushort)(Tmp & 0x3FF));
            //    Locals = (ushort)((Tmp >> 10) & 0x3F);

            //    if ((Locals <= MAX_COMMAND_LOCALS) && (Globals <= MAX_COMMAND_GLOBALS))
            //    {

            //        pImgHead = (IMGHEAD*)GH.ComInstance.Image;
            //        pObjHead = (OBJHEAD*)(GH.ComInstance.Image.Skip(IMGHEAD.SizeOf));

            //        (*pImgHead).Sign[0] = 'l';
            //        (*pImgHead).Sign[1] = 'e';
            //        (*pImgHead).Sign[2] = 'g';
            //        (*pImgHead).Sign[3] = 'o';
            //        (*pImgHead).VersionInfo = (UWORD)(VERS * 100.0);
            //        (*pImgHead).NumberOfObjects = (OBJID)1;
            //        (*pImgHead).GlobalBytes = (GBINDEX)Globals;

            //        (*pObjHead).OffsetToInstructions = (IP)(sizeof(IMGHEAD) + sizeof(OBJHEAD));
            //        (*pObjHead).OwnerObjectId = 0;
            //        (*pObjHead).TriggerCount = 0;
            //        (*pObjHead).LocalBytes = (OBJID)Locals;

            //        memcpy(&ComInstance.Image[sizeof(IMGHEAD) + sizeof(OBJHEAD)], (*pDirCmd).Code, Length);
            //        Length += sizeof(IMGHEAD) + sizeof(OBJHEAD);

            //        ComInstance.Image[Length] = opOBJECT_END;
            //        (*pImgHead).ImageSize = Length;

            //        Result = 1;
            //    }
            //    else
            //    {
            //        (*pComRpl).Cmd = DIRECT_REPLY_ERROR;
            //    }
            //}
            //else
            //{
            //    (*pComRpl).Cmd = DIRECT_REPLY_ERROR;
            //}

            return (Result);
        }

        void cComSystemReply(RXBUF pRxBuf, TXBUF pTxBuf)
        {
            COMCMD pComCmd;
            SYSCMDC pSysCmdC;
            CMDSIZE CmdSize;
            UBYTE[] FileName = new byte[MAX_FILENAME_SIZE];

            // TODO: SYSTEM COMMAND anime
            //pComCmd = (COMCMD*)pRxBuf->Buf;
            //pSysCmdC = (SYSCMDC*)(*pComCmd).PayLoad;
            //CmdSize = (*pComCmd).CmdSize;

            //switch ((*pSysCmdC).Sys)
            //{
            //    case BEGIN_DOWNLOAD:
            //        {
            //            // This is the reply from the begin download command
            //            // This is part of the file transfer sequence
            //            // Check for single file or Folder transfer

            //            RPLY_BEGIN_DL* pRplyBeginDl;

            //            pRplyBeginDl = (RPLY_BEGIN_DL*)&(pRxBuf->Buf[0]);

            //            if ((SUCCESS == pRplyBeginDl->Status) || (END_OF_FILE == pRplyBeginDl->Status))
            //            {
            //                pTxBuf->RemoteFileHandle = pRplyBeginDl->Handle;

            //                if (FILE_IN_PROGRESS_WAIT_FOR_REPLY == pTxBuf->SubState)
            //                {
            //                    // Issue continue write as file is not
            //                    // completely downloaded
            //                    cComCreateContinueDl(pRxBuf, pTxBuf);
            //                }
            //                else
            //                {
            //                    if (FILE_COMPLETE_WAIT_FOR_REPLY == pTxBuf->SubState)
            //                    {
            //                        // Complete file downloaded check for more files
            //                        // to download
            //                        if (TXFOLDER == pTxBuf->State)
            //                        {
            //                            if (cComGetNxtFile(pTxBuf->pDir, FileName))
            //                            {
            //                                cComCreateBeginDl(pTxBuf, FileName);
            //                            }
            //                            else
            //                            {
            //                                // No More files
            //                                pTxBuf->State = TXIDLE;
            //                                pTxBuf->SubState = SUBSTATE_IDLE;
            //                                ComInstance.ComResult = OK;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            // Only one file to send
            //                            pTxBuf->State = TXIDLE;
            //                            pTxBuf->SubState = SUBSTATE_IDLE;
            //                            ComInstance.ComResult = OK;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        break;

            //    case CONTINUE_DOWNLOAD:
            //        {

            //            RPLY_CONTINUE_DL* pRplyContinueDl;
            //            pRplyContinueDl = (RPLY_CONTINUE_DL*)&(pRxBuf->Buf[0]);

            //            if ((SUCCESS == pRplyContinueDl->Status) || (END_OF_FILE == pRplyContinueDl->Status))
            //            {
            //                if (FILE_IN_PROGRESS_WAIT_FOR_REPLY == pTxBuf->SubState)
            //                {
            //                    // Issue continue write as file is not
            //                    // completely downloaded
            //                    cComCreateContinueDl(pRxBuf, pTxBuf);
            //                }
            //                else
            //                {
            //                    if (FILE_COMPLETE_WAIT_FOR_REPLY == pTxBuf->SubState)
            //                    {
            //                        // Complete file downloaded check for more files
            //                        // to download
            //                        if (TXFOLDER == pTxBuf->State)
            //                        {
            //                            if (cComGetNxtFile(pTxBuf->pDir, FileName))
            //                            {
            //                                cComCreateBeginDl(pTxBuf, FileName);
            //                            }
            //                            else
            //                            {
            //                                pTxBuf->State = TXIDLE;
            //                                pTxBuf->SubState = SUBSTATE_IDLE;
            //                                ComInstance.ComResult = OK;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pTxBuf->State = TXIDLE;
            //                            pTxBuf->SubState = SUBSTATE_IDLE;
            //                            ComInstance.ComResult = OK;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        break;

            //    default:
            //        {
            //        }
            //        break;
            //}
        }

        void cComSystemCommand(RXBUF pRxBuf, TXBUF pTxBuf)
        {
            COMCMD pComCmd;
            SYSCMDC pSysCmdC;
            CMDSIZE CmdSize;
            int Tmp;
            char[] Folder = new char[60];
            ULONG BytesToWrite;
            DATA8 FileHandle;

            // TODO: SYSTEM COMMAND SHITE anime
            //            pComCmd = (COMCMD*)pRxBuf->Buf;
            //            pSysCmdC = (SYSCMDC*)(*pComCmd).PayLoad;
            //            CmdSize = (*pComCmd).CmdSize;

            //            switch ((*pSysCmdC).Sys)
            //            {
            //                case BEGIN_DOWNLOAD:
            //                    {
            //                        BEGIN_DL* pBeginDl;
            //                        RPLY_BEGIN_DL* pReplyDl;
            //                        ULONG MsgHeaderSize;

            //                        //Setup pointers
            //                        pBeginDl = (BEGIN_DL*)pRxBuf->Buf;
            //                        pReplyDl = (RPLY_BEGIN_DL*)pTxBuf->Buf;

            //                        // Get file handle
            //                        FileHandle = cComGetHandle((char*)pBeginDl->Path);
            //                        pRxBuf->pFile = &(ComInstance.Files[FileHandle]);

            //                        // Fill the reply
            //                        pReplyDl->CmdSize = SIZEOF_RPLYBEGINDL - sizeof(CMDSIZE);
            //                        pReplyDl->MsgCount = pBeginDl->MsgCount;
            //                        pReplyDl->CmdType = SYSTEM_REPLY;
            //                        pReplyDl->Cmd = BEGIN_DOWNLOAD;
            //                        pReplyDl->Status = SUCCESS;
            //                        pReplyDl->Handle = FileHandle;

            //                        if (FileHandle >= 0)
            //                        {
            //                            pRxBuf->pFile->Size = (ULONG)(pBeginDl->FileSizeLsb);
            //                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeNsb1) << 8;
            //                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeNsb2) << 16;
            //                            pRxBuf->pFile->Size += (ULONG)(pBeginDl->FileSizeMsb) << 24;

            //                            if (OK == cComCheckForSpace(&(ComInstance.Files[FileHandle].Name[0]), pRxBuf->pFile->Size))
            //                            {

            //                                pRxBuf->pFile->Length = (ULONG)0;
            //                                pRxBuf->pFile->Pointer = (ULONG)0;

            //                                Tmp = 0;
            //                                while ((ComInstance.Files[FileHandle].Name[Tmp]) && (Tmp < (MAX_FILENAME_SIZE - 1)))
            //                                {
            //                                    Folder[Tmp] = ComInstance.Files[FileHandle].Name[Tmp];
            //                                    if (Folder[Tmp] == '/')
            //                                    {
            //                                        Folder[Tmp + 1] = 0;
            //                                        if (strcmp("~/", Folder) != 0)
            //                                        {
            //                                            if (mkdir(Folder, S_IRWXU | S_IRWXG | S_IRWXO) == 0)
            //                                            {
            //                                                chmod(Folder, S_IRWXU | S_IRWXG | S_IRWXO);
            //# ifdef DEBUG
            //                                                printf("Folder %s created\r\n", Folder);
            //#endif
            //                                            }
            //                                            else
            //                                            {
            //# ifdef DEBUG
            //                                                printf("Folder %s not created (%s)\r\n", Folder, strerror(errno));
            //#endif
            //                                            }
            //                                        }
            //                                    }
            //                                    Tmp++;
            //                                }

            //                                pRxBuf->pFile->File = open(pRxBuf->pFile->Name, O_CREAT | O_WRONLY | O_TRUNC | O_SYNC, 0x666);

            //                                if (pRxBuf->pFile->File >= 0)
            //                                {
            //                                    MsgHeaderSize = (strlen((char*)pBeginDl->Path) + 1 + SIZEOF_BEGINDL); // +1 = zero termination
            //                                    pRxBuf->MsgLen = CmdSize + sizeof(CMDSIZE) - MsgHeaderSize;

            //                                    if (CmdSize <= (pRxBuf->BufSize - sizeof(CMDSIZE)))
            //                                    {
            //                                        // All data included in this buffer
            //                                        BytesToWrite = pRxBuf->MsgLen;
            //                                        pRxBuf->State = RXIDLE;
            //                                    }
            //                                    else
            //                                    {
            //                                        // Rx buffer must be read more that one to receive the complete message
            //                                        BytesToWrite = pRxBuf->BufSize - MsgHeaderSize;
            //                                        pRxBuf->State = RXFILEDL;
            //                                    }
            //                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

            //                                    if (BytesToWrite > (ComInstance.Files[FileHandle].Size))
            //                                    {
            //                                        // If Bytes to write into the file is bigger than file size -> Error message
            //                                        pReplyDl->Status = SIZE_ERROR;
            //                                        BytesToWrite = ComInstance.Files[FileHandle].Size;
            //                                    }

            //                                    write(pRxBuf->pFile->File, &(pRxBuf->Buf[MsgHeaderSize]), (size_t)BytesToWrite);
            //                                    ComInstance.Files[FileHandle].Length += (ULONG)BytesToWrite;
            //                                    pRxBuf->RxBytes = (ULONG)BytesToWrite;
            //                                    pRxBuf->pFile->Pointer = (ULONG)BytesToWrite;

            //                                    if (pRxBuf->pFile->Pointer >= pRxBuf->pFile->Size)
            //                                    {
            //                                        cComCloseFileHandle(&(pRxBuf->pFile->File));
            //                                        chmod(ComInstance.Files[FileHandle].Name, S_IRWXU | S_IRWXG | S_IRWXO);
            //                                        cComFreeHandle(FileHandle);

            //                                        pReplyDl->Status = END_OF_FILE;
            //                                        pRxBuf->State = RXIDLE;
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    // Error in opening file
            //# ifdef DEBUG
            //                                    printf("File %s not created\r\n", ComInstance.Files[FileHandle].Name);
            //#endif
            //                                    cComFreeHandle(FileHandle);
            //                                    pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
            //                                    pReplyDl->Status = UNKNOWN_HANDLE;
            //                                    pReplyDl->Handle = -1;
            //                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

            //                                }
            //                            }
            //                            else
            //                            {
            //                                //Not enough space for the file
            //# ifdef DEBUG
            //                                printf("File %s is too big\r\n", ComInstance.Files[FileHandle].Name);
            //#endif

            //                                cComFreeHandle(FileHandle);
            //                                pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
            //                                pReplyDl->Status = SIZE_ERROR;
            //                                pReplyDl->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            // Error in getting a handle
            //                            pReplyDl->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyDl->Status = NO_HANDLES_AVAILABLE;
            //                            pReplyDl->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYBEGINDL;

            //# ifdef DEBUG
            //                            printf("No more handles\r\n");
            //#endif
            //                        }
            //                    }
            //                    break;

            //                case CONTINUE_DOWNLOAD:
            //                    {
            //                        CONTINUE_DL* pContiDl;
            //                        RPLY_CONTINUE_DL* pRplyContiDl;
            //                        ULONG BytesToWrite;

            //                        //Setup pointers
            //                        pContiDl = (CONTINUE_DL*)pComCmd;
            //                        pRplyContiDl = (RPLY_CONTINUE_DL*)pTxBuf->Buf;

            //                        // Get handle
            //                        FileHandle = pContiDl->Handle;

            //                        // Fill the reply
            //                        pRplyContiDl->CmdSize = SIZEOF_RPLYCONTINUEDL - sizeof(CMDSIZE);
            //                        pRplyContiDl->MsgCount = pContiDl->MsgCount;
            //                        pRplyContiDl->CmdType = SYSTEM_REPLY;
            //                        pRplyContiDl->Cmd = CONTINUE_DOWNLOAD;
            //                        pRplyContiDl->Status = SUCCESS;
            //                        pRplyContiDl->Handle = FileHandle;

            //                        if ((FileHandle >= 0) && (ComInstance.Files[FileHandle].Name[0]))
            //                        {

            //                            pRxBuf->FileHandle = FileHandle;
            //                            pRxBuf->pFile = &(ComInstance.Files[FileHandle]);

            //                            if (pRxBuf->pFile->File >= 0)
            //                            {

            //                                pRxBuf->MsgLen = (pContiDl->CmdSize + sizeof(CMDSIZE)) - SIZEOF_CONTINUEDL;
            //                                pRxBuf->RxBytes = 0;

            //                                if (pContiDl->CmdSize <= (pRxBuf->BufSize - sizeof(CMDSIZE)))
            //                                {
            //                                    // All data included in this buffer
            //                                    BytesToWrite = pRxBuf->MsgLen;
            //                                    pRxBuf->State = RXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    // Rx buffer must be read more that one to receive the complete message
            //                                    BytesToWrite = pRxBuf->BufSize - SIZEOF_CONTINUEDL;
            //                                    pRxBuf->State = RXFILEDL;
            //                                }
            //                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;

            //                                if ((pRxBuf->pFile->Pointer + pRxBuf->MsgLen) > pRxBuf->pFile->Size)
            //                                {
            //                                    BytesToWrite = pRxBuf->pFile->Size - pRxBuf->pFile->Pointer;

            //# ifdef DEBUG
            //                                    printf("File size limited\r\n");
            //#endif
            //                                }

            //                                write(ComInstance.Files[FileHandle].File, (pContiDl->PayLoad), (size_t)BytesToWrite);
            //                                pRxBuf->pFile->Pointer += BytesToWrite;
            //                                pRxBuf->RxBytes = BytesToWrite;

            //# ifdef DEBUG
            //                                printf("Size %8lu - Loaded %8lu\r\n", (unsigned long)ComInstance.Files[FileHandle].Size,(unsigned long)ComInstance.Files[FileHandle].Length);
            //#endif

            //                                if (pRxBuf->pFile->Pointer >= ComInstance.Files[FileHandle].Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("%s %lu bytes downloaded\r\n", ComInstance.Files[FileHandle].Name, (unsigned long)ComInstance.Files[FileHandle].Length);
            //#endif

            //                                    cComCloseFileHandle(&(ComInstance.Files[FileHandle].File));
            //                                    chmod(ComInstance.Files[FileHandle].Name, S_IRWXU | S_IRWXG | S_IRWXO);
            //                                    cComFreeHandle(FileHandle);
            //                                    pRplyContiDl->Status = END_OF_FILE;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                // Illegal file
            //                                pRplyContiDl->CmdType = SYSTEM_REPLY_ERROR;
            //                                pRplyContiDl->Status = UNKNOWN_ERROR;
            //                                pRplyContiDl->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;
            //                                cComFreeHandle(FileHandle);

            //# ifdef DEBUG
            //                                printf("Data not appended\r\n");
            //#endif
            //                            }
            //                        }
            //                        else
            //                        {
            //                            // Illegal file handle
            //                            pRplyContiDl->CmdType = SYSTEM_REPLY_ERROR;
            //                            pRplyContiDl->Status = UNKNOWN_HANDLE;
            //                            pRplyContiDl->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEDL;

            //# ifdef DEBUG
            //                            printf("Invalid handle\r\n");
            //#endif
            //                        }
            //                    }
            //                    break;

            //                case BEGIN_UPLOAD:
            //                    {
            //                        ULONG BytesToRead;
            //                        ULONG ReadBytes;

            //                        BEGIN_READ* pBeginRead; ;
            //                        RPLY_BEGIN_READ* pReplyBeginRead;

            //                        //Setup pointers
            //                        pBeginRead = (BEGIN_READ*)pRxBuf->Buf;
            //                        pReplyBeginRead = (RPLY_BEGIN_READ*)pTxBuf->Buf;

            //                        FileHandle = cComGetHandle((char*)pBeginRead->Path);

            //                        pTxBuf->pFile = &ComInstance.Files[FileHandle];  // Insert the file pointer into the ch struct
            //                        pTxBuf->FileHandle = FileHandle;                      // Also save the File handle number

            //                        pReplyBeginRead->CmdSize = SIZEOF_RPLYBEGINREAD - sizeof(CMDSIZE);
            //                        pReplyBeginRead->MsgCount = pBeginRead->MsgCount;
            //                        pReplyBeginRead->CmdType = SYSTEM_REPLY;
            //                        pReplyBeginRead->Cmd = BEGIN_UPLOAD;
            //                        pReplyBeginRead->Status = SUCCESS;
            //                        pReplyBeginRead->Handle = FileHandle;

            //                        if (FileHandle >= 0)
            //                        {
            //                            BytesToRead = (ULONG)(pBeginRead->BytesToReadLsb);
            //                            BytesToRead += (ULONG)(pBeginRead->BytesToReadMsb) << 8;

            //                            pTxBuf->pFile->File = open(pTxBuf->pFile->Name, O_RDONLY, 0x444);

            //                            if (pTxBuf->pFile->File >= 0)
            //                            {
            //                                // Get file length
            //                                pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
            //                                lseek(pTxBuf->pFile->File, 0L, SEEK_SET);

            //                                pTxBuf->MsgLen = BytesToRead;

            //                                if (BytesToRead > pTxBuf->pFile->Size)
            //                                {
            //                                    // if message length  > filesize then crop message
            //                                    pTxBuf->MsgLen = pTxBuf->pFile->Size;
            //                                }

            //                                if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINREAD) <= pTxBuf->BufSize)
            //                                {
            //                                    // Read all requested bytes as they can fit into the buffer
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyBeginRead->PayLoad, pTxBuf->MsgLen);
            //                                    pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYBEGINREAD;
            //                                    pTxBuf->State = TXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    // Read only up to full buffer size
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyBeginRead->PayLoad, (pTxBuf->BufSize - SIZEOF_RPLYBEGINREAD));
            //                                    pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYBEGINREAD;
            //                                    pTxBuf->State = TXFILEUPLOAD;
            //                                }

            //                                pTxBuf->SendBytes = ReadBytes;  // No of bytes send in message
            //                                pTxBuf->pFile->Pointer = ReadBytes;  // No of bytes read from file

            //# ifdef DEBUG
            //                                printf("Bytes to read %d bytes read %d\r\n", BytesToRead, ReadBytes);
            //                                printf("FileSize: %d \r\n", pTxBuf->pFile->Size);
            //#endif

            //                                pReplyBeginRead->CmdSize += (CMDSIZE)pTxBuf->MsgLen;
            //                                pReplyBeginRead->FileSizeLsb = (UBYTE)(pTxBuf->pFile->Size);
            //                                pReplyBeginRead->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
            //                                pReplyBeginRead->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
            //                                pReplyBeginRead->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

            //                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                    pReplyBeginRead->Status = END_OF_FILE;
            //                                    cComCloseFileHandle(&(pTxBuf->pFile->File));
            //                                    cComFreeHandle(FileHandle);
            //                                }

            //# ifdef DEBUG
            //                                cComPrintTxMsg(pTxBuf);
            //#endif

            //                            }
            //                            else
            //                            {
            //                                pReplyBeginRead->CmdType = SYSTEM_REPLY_ERROR;
            //                                pReplyBeginRead->Status = UNKNOWN_HANDLE;
            //                                pReplyBeginRead->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYBEGINREAD;
            //                                cComFreeHandle(FileHandle);

            //# ifdef DEBUG
            //                                printf("File %s is not present \r\n", ComInstance.Files[FileHandle].Name);
            //#endif
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pReplyBeginRead->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyBeginRead->Status = NO_HANDLES_AVAILABLE;
            //                            pReplyBeginRead->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYBEGINREAD;

            //# ifdef DEBUG
            //                            printf("No more handles\r\n");
            //#endif
            //                        }

            //                    }
            //                    break;

            //                case CONTINUE_UPLOAD:
            //                    {
            //                        ULONG BytesToRead;
            //                        ULONG ReadBytes;

            //                        CONTINUE_READ* pContinueRead;
            //                        RPLY_CONTINUE_READ* pReplyContinueRead;

            //                        //Setup pointers
            //                        pContinueRead = (CONTINUE_READ*)pRxBuf->Buf;
            //                        pReplyContinueRead = (RPLY_CONTINUE_READ*)pTxBuf->Buf;

            //                        FileHandle = pContinueRead->Handle;

            //                        /* Fill out the default settings */
            //                        pReplyContinueRead->CmdSize = SIZEOF_RPLYCONTINUEREAD - sizeof(CMDSIZE);
            //                        pReplyContinueRead->MsgCount = pContinueRead->MsgCount;
            //                        pReplyContinueRead->CmdType = SYSTEM_REPLY;
            //                        pReplyContinueRead->Cmd = CONTINUE_UPLOAD;
            //                        pReplyContinueRead->Status = SUCCESS;
            //                        pReplyContinueRead->Handle = FileHandle;

            //                        if ((FileHandle >= 0) && (pTxBuf->pFile->Name[0]))
            //                        {

            //                            if (pTxBuf->pFile->File >= 0)
            //                            {
            //                                BytesToRead = (ULONG)(pContinueRead->BytesToReadLsb);
            //                                BytesToRead += (ULONG)(pContinueRead->BytesToReadMsb) << 8;

            //                                // If host is asking for more bytes than remaining file size then
            //                                // message length needs to adjusted accordingly
            //                                if ((pTxBuf->pFile->Size - pTxBuf->pFile->Pointer) > BytesToRead)
            //                                {
            //                                    pTxBuf->MsgLen = BytesToRead;
            //                                }
            //                                else
            //                                {
            //                                    pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
            //                                    BytesToRead = pTxBuf->MsgLen;
            //                                }

            //                                if ((BytesToRead + SIZEOF_RPLYCONTINUEREAD) <= pTxBuf->BufSize)
            //                                {
            //                                    // Read all requested bytes as they can fit into the buffer
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueRead->PayLoad, (size_t)BytesToRead);
            //                                    pTxBuf->BlockLen = BytesToRead + SIZEOF_RPLYCONTINUEREAD;
            //                                    pTxBuf->State = TXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueRead->PayLoad, (size_t)(pTxBuf->BufSize - SIZEOF_RPLYCONTINUEREAD));
            //                                    pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYCONTINUEREAD;
            //                                    pTxBuf->State = TXFILEUPLOAD;
            //                                }

            //                                pTxBuf->SendBytes = (ULONG)ReadBytes;
            //                                pTxBuf->pFile->Pointer += (ULONG)ReadBytes;
            //                                pReplyContinueRead->CmdSize += pTxBuf->MsgLen;

            //# ifdef DEBUG
            //                                cComPrintTxMsg(pTxBuf);
            //#endif

            //# ifdef DEBUG
            //                                printf("Size %8lu - Loaded %8lu\r\n", (unsigned long)pTxBuf->pFile->Size,(unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                if (ComInstance.Files[FileHandle].Pointer >= ComInstance.Files[FileHandle].Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("%s %lu bytes UpLoaded\r\n", ComInstance.Files[FileHandle].Name, (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                    pReplyContinueRead->Status = END_OF_FILE;
            //                                    cComCloseFileHandle(&(pTxBuf->pFile->File));
            //                                    cComFreeHandle(FileHandle);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                pReplyContinueRead->CmdType = SYSTEM_REPLY_ERROR;
            //                                pReplyContinueRead->Status = HANDLE_NOT_READY;
            //                                pReplyContinueRead->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEREAD;
            //                                cComFreeHandle(FileHandle);
            //# ifdef DEBUG
            //                                printf("Data not read\r\n");
            //#endif
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pReplyContinueRead->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyContinueRead->Status = UNKNOWN_HANDLE;
            //                            pReplyContinueRead->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEREAD;
            //# ifdef DEBUG
            //                            printf("Invalid handle\r\n");
            //#endif
            //                        }
            //                    }
            //                    break;

            //                case BEGIN_GETFILE:
            //                    {

            //                        ULONG BytesToRead;
            //                        ULONG ReadBytes;
            //                        BEGIN_GET_FILE* pBeginGetFile; ;
            //                        RPLY_BEGIN_GET_FILE* pReplyBeginGetFile;

            //                        //Setup pointers
            //                        pBeginGetFile = (BEGIN_GET_FILE*)pRxBuf->Buf;
            //                        pReplyBeginGetFile = (RPLY_BEGIN_GET_FILE*)pTxBuf->Buf;

            //                        FileHandle = cComGetHandle((char*)pBeginGetFile->Path);
            //                        pTxBuf->pFile = &ComInstance.Files[FileHandle];  // Insert the file pointer into the ch struct
            //                        pTxBuf->FileHandle = FileHandle;                      // Also save the File handle number

            //                        // Fill out the reply
            //                        pReplyBeginGetFile->CmdSize = SIZEOF_RPLYBEGINGETFILE - sizeof(CMDSIZE);
            //                        pReplyBeginGetFile->MsgCount = pBeginGetFile->MsgCount;
            //                        pReplyBeginGetFile->CmdType = SYSTEM_REPLY;
            //                        pReplyBeginGetFile->Cmd = BEGIN_GETFILE;
            //                        pReplyBeginGetFile->Handle = FileHandle;
            //                        pReplyBeginGetFile->Status = SUCCESS;

            //                        if (FileHandle >= 0)
            //                        {
            //                            /* How many bytes to be returned the in the reply for BEGIN_UPLOAD    */
            //                            /* Should actually only be 2 bytes as only we can hold into 2 length  */
            //                            /* bytes in the protocol                                              */
            //                            BytesToRead = (ULONG)(pBeginGetFile->BytesToReadLsb);
            //                            BytesToRead += (ULONG)(pBeginGetFile->BytesToReadMsb) << 8;

            //# ifdef DEBUG
            //                            printf("File to get:  %s \r\n", ComInstance.Files[FileHandle].Name);
            //#endif

            //                            pTxBuf->pFile->File = open(pTxBuf->pFile->Name, O_RDONLY, 0x444);
            //                            if (pTxBuf->pFile->File >= 0)
            //                            {
            //                                // Get file length
            //                                pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
            //                                lseek(pTxBuf->pFile->File, 0L, SEEK_SET);

            //                                pTxBuf->MsgLen = BytesToRead;
            //                                if (BytesToRead > pTxBuf->pFile->Size)
            //                                {
            //                                    // if message length  > filesize then crop message
            //                                    pTxBuf->MsgLen = pTxBuf->pFile->Size;
            //                                }

            //                                if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINGETFILE) <= pTxBuf->BufSize)
            //                                {
            //                                    // Read all requested bytes as they can fit into the buffer
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyBeginGetFile->PayLoad, pTxBuf->MsgLen);
            //                                    pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYBEGINGETFILE;
            //                                    pTxBuf->State = TXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    // Read only up to full buffer size
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyBeginGetFile->PayLoad, (pTxBuf->BufSize - SIZEOF_RPLYBEGINGETFILE));
            //                                    pTxBuf->BlockLen = pTxBuf->BufSize - SIZEOF_RPLYBEGINGETFILE;
            //                                    pTxBuf->State = TXGETFILE;
            //                                }

            //                                pTxBuf->SendBytes = ReadBytes;  // No of bytes send in message
            //                                pTxBuf->pFile->Pointer = ReadBytes;  // No of bytes read from file

            //# ifdef DEBUG
            //                                printf("Bytes to read %d vs. bytes read %d\r\n", BytesToRead, ReadBytes);
            //                                printf("FileSize: %d \r\n", pTxBuf->pFile->Size);
            //#endif

            //                                pReplyBeginGetFile->CmdSize += pTxBuf->MsgLen;
            //                                pReplyBeginGetFile->FileSizeLsb = (UBYTE)pTxBuf->pFile->Size;
            //                                pReplyBeginGetFile->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
            //                                pReplyBeginGetFile->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
            //                                pReplyBeginGetFile->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

            //                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                    // If last bytes has been returned and file is not open for writing
            //                                    if (FAIL == cMemoryCheckOpenWrite(pTxBuf->pFile->Name))
            //                                    {
            //                                        pReplyBeginGetFile->Status = END_OF_FILE;
            //                                        cComCloseFileHandle(&(pTxBuf->pFile->File));
            //                                        cComFreeHandle(FileHandle);
            //                                    }
            //                                }

            //# ifdef DEBUG
            //                                cComPrintTxMsg(pTxBuf);
            //#endif
            //                            }
            //                            else
            //                            {
            //                                pReplyBeginGetFile->CmdType = SYSTEM_REPLY_ERROR;
            //                                pReplyBeginGetFile->Status = HANDLE_NOT_READY;
            //                                pReplyBeginGetFile->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYBEGINGETFILE;
            //                                cComFreeHandle(FileHandle);

            //# ifdef DEBUG
            //                                printf("File %s is not present \r\n", ComInstance.Files[FileHandle].Name);
            //#endif
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pReplyBeginGetFile->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyBeginGetFile->Status = UNKNOWN_HANDLE;
            //                            pReplyBeginGetFile->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYBEGINGETFILE;

            //# ifdef DEBUG
            //                            printf("No more handles\r\n");
            //#endif
            //                        }
            //                    }
            //                    break;

            //                case CONTINUE_GETFILE:
            //                    {
            //                        ULONG BytesToRead;
            //                        ULONG ReadBytes;
            //                        CONTINUE_GET_FILE* pContinueGetFile;
            //                        RPLY_CONTINUE_GET_FILE* pReplyContinueGetFile;

            //                        //Setup pointers
            //                        pContinueGetFile = (CONTINUE_GET_FILE*)pRxBuf->Buf;
            //                        pReplyContinueGetFile = (RPLY_CONTINUE_GET_FILE*)pTxBuf->Buf;

            //                        FileHandle = pContinueGetFile->Handle;

            //                        /* Fill out the default settings */
            //                        pReplyContinueGetFile->CmdSize = SIZEOF_RPLYCONTINUEGETFILE - sizeof(CMDSIZE);
            //                        pReplyContinueGetFile->MsgCount = pContinueGetFile->MsgCount;
            //                        pReplyContinueGetFile->CmdType = SYSTEM_REPLY;
            //                        pReplyContinueGetFile->Cmd = CONTINUE_GETFILE;
            //                        pReplyContinueGetFile->Status = SUCCESS;
            //                        pReplyContinueGetFile->Handle = FileHandle;

            //                        if ((FileHandle >= 0) && (pTxBuf->pFile->Name[0]))
            //                        {

            //                            if (pTxBuf->pFile->File >= 0)
            //                            {

            //                                BytesToRead = (ULONG)(pContinueGetFile->BytesToReadLsb);
            //                                BytesToRead += (ULONG)(pContinueGetFile->BytesToReadMsb) << 8;

            //                                // Get new file length: Set pointer to 0 -> find end -> set where to read from
            //                                lseek(pTxBuf->pFile->File, 0L, SEEK_SET);
            //                                pTxBuf->pFile->Size = lseek(pTxBuf->pFile->File, 0L, SEEK_END);
            //                                lseek(pTxBuf->pFile->File, pTxBuf->pFile->Pointer, SEEK_SET);

            //                                // If host is asking for more bytes than remaining file size then
            //                                // message length needs to adjusted accordingly
            //                                if ((pTxBuf->pFile->Size - pTxBuf->pFile->Pointer) > BytesToRead)
            //                                {
            //                                    pTxBuf->MsgLen = BytesToRead;
            //                                }
            //                                else
            //                                {
            //                                    pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
            //                                    BytesToRead = pTxBuf->MsgLen;
            //                                }

            //                                if ((BytesToRead + SIZEOF_RPLYCONTINUEGETFILE) <= pTxBuf->BufSize)
            //                                {
            //                                    // Read all requested bytes as they can fit into the buffer
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueGetFile->PayLoad, (size_t)BytesToRead);
            //                                    pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYCONTINUEGETFILE;
            //                                    pTxBuf->State = TXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    ReadBytes = read(pTxBuf->pFile->File, pReplyContinueGetFile->PayLoad, (size_t)(pTxBuf->BufSize - SIZEOF_RPLYCONTINUEGETFILE));
            //                                    pTxBuf->BlockLen = ReadBytes + SIZEOF_RPLYCONTINUEGETFILE;
            //                                    pTxBuf->State = TXGETFILE;
            //                                }

            //                                pTxBuf->SendBytes = (ULONG)ReadBytes;
            //                                pTxBuf->pFile->Pointer += (ULONG)ReadBytes;

            //                                pReplyContinueGetFile->CmdSize += pTxBuf->MsgLen;
            //                                pReplyContinueGetFile->FileSizeLsb = (UBYTE)(pTxBuf->pFile->Size);
            //                                pReplyContinueGetFile->FileSizeNsb1 = (UBYTE)(pTxBuf->pFile->Size >> 8);
            //                                pReplyContinueGetFile->FileSizeNsb2 = (UBYTE)(pTxBuf->pFile->Size >> 16);
            //                                pReplyContinueGetFile->FileSizeMsb = (UBYTE)(pTxBuf->pFile->Size >> 24);

            //                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                    // If last bytes has been returned and file is not open for writing
            //                                    if (FAIL == cMemoryCheckOpenWrite(pTxBuf->pFile->Name))
            //                                    {
            //                                        pReplyContinueGetFile->Status = END_OF_FILE;
            //                                        cComCloseFileHandle(&(pTxBuf->pFile->File));
            //                                        cComFreeHandle(FileHandle);
            //                                    }
            //                                }

            //# ifdef DEBUG
            //                                printf("Size %8lu - Loaded %8lu\r\n", (unsigned long)pTxBuf->pFile->Size,(unsigned long)pTxBuf->pFile->Length);
            //#endif

            //# ifdef DEBUG
            //                                cComPrintTxMsg(pTxBuf);
            //#endif

            //                            }
            //                            else
            //                            {
            //                                pReplyContinueGetFile->CmdType = SYSTEM_REPLY_ERROR;
            //                                pReplyContinueGetFile->Status = HANDLE_NOT_READY;
            //                                pReplyContinueGetFile->Handle = -1;
            //                                pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEGETFILE;
            //                                cComFreeHandle(FileHandle);

            //# ifdef DEBUG
            //                                printf("Data not read\r\n");
            //#endif
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pReplyContinueGetFile->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyContinueGetFile->Status = UNKNOWN_HANDLE;
            //                            pReplyContinueGetFile->Handle = -1;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYCONTINUEGETFILE;

            //# ifdef DEBUG
            //                            printf("Invalid handle\r\n");
            //#endif
            //                        }
            //                    }
            //                    break;

            //                case LIST_FILES:
            //                    {
            //                        ULONG BytesToRead;
            //                        ULONG Len;
            //                        char TmpFileName[MD5LEN + 1 + SIZEOFFILESIZE + 1 + FILENAMESIZE];
            //                        BEGIN_LIST* pBeginList;
            //                        RPLY_BEGIN_LIST* pReplyBeginList;

            //                        //Setup pointers
            //                        pBeginList = (BEGIN_LIST*)pRxBuf->Buf;
            //                        pReplyBeginList = (RPLY_BEGIN_LIST*)pTxBuf->Buf;

            //                        FileHandle = cComGetHandle("ListFileHandle");
            //                        pTxBuf->pFile = &ComInstance.Files[FileHandle];   // Insert the file pointer into the ch struct
            //                        pTxBuf->FileHandle = FileHandle;                       // Also save the File handle number

            //                        pReplyBeginList->CmdSize = SIZEOF_RPLYBEGINLIST - sizeof(CMDSIZE);
            //                        pReplyBeginList->MsgCount = pBeginList->MsgCount;
            //                        pReplyBeginList->CmdType = SYSTEM_REPLY;
            //                        pReplyBeginList->Cmd = LIST_FILES;
            //                        pReplyBeginList->Status = SUCCESS;
            //                        pReplyBeginList->Handle = FileHandle;
            //                        pTxBuf->MsgLen = 0;

            //                        if (0 <= FileHandle)
            //                        {
            //                            BytesToRead = (ULONG)pBeginList->BytesToReadLsb;
            //                            BytesToRead += (ULONG)pBeginList->BytesToReadMsb << 8;

            //                            snprintf((char*)pTxBuf->Folder, FILENAMESIZE, "%s", (char*)pBeginList->Path);
            //                            pTxBuf->pFile->File = scandir((char*)pTxBuf->Folder, &(pTxBuf->pFile->namelist), 0, NULL);

            //                            if (pTxBuf->pFile->File <= 0)
            //                            {
            //                                if (pTxBuf->pFile->File == 0)
            //                                {
            //                                    // here if no files found, equal to error as "./" and "../" normally
            //                                    // always found
            //                                    pReplyBeginList->ListSizeLsb = 0;
            //                                    pReplyBeginList->ListSizeNsb1 = 0;
            //                                    pReplyBeginList->ListSizeNsb2 = 0;
            //                                    pReplyBeginList->ListSizeMsb = 0;
            //                                    pReplyBeginList->Handle = -1;

            //                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
            //                                    cComFreeHandle(FileHandle);
            //                                }
            //                                else
            //                                {
            //                                    // If error occurs
            //                                    pReplyBeginList->CmdType = SYSTEM_REPLY_ERROR;
            //                                    pReplyBeginList->Status = UNKNOWN_ERROR;
            //                                    pReplyBeginList->ListSizeLsb = 0;
            //                                    pReplyBeginList->ListSizeNsb1 = 0;
            //                                    pReplyBeginList->ListSizeNsb2 = 0;
            //                                    pReplyBeginList->ListSizeMsb = 0;
            //                                    pReplyBeginList->Handle = -1;

            //                                    pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
            //                                    cComFreeHandle(FileHandle);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                SLONG TmpN;
            //                                ULONG NameLen;
            //                                ULONG BytesToSend;
            //                                UBYTE Repeat;
            //                                UBYTE* pDstAdr;
            //                                UBYTE* pSrcAdr;

            //                                TmpN = pTxBuf->pFile->File;     // Make a copy of number of entries
            //                                Len = 0;

            //                                // Start by calculating the length of the entire list
            //                                while (TmpN--)
            //                                {
            //                                    if ((DT_REG == pTxBuf->pFile->namelist[TmpN]->d_type) || (DT_DIR == pTxBuf->pFile->namelist[TmpN]->d_type) || (DT_LNK == pTxBuf->pFile->namelist[TmpN]->d_type))
            //                                    {
            //                                        Len += strlen(pTxBuf->pFile->namelist[TmpN]->d_name);
            //                                        if (DT_REG != pTxBuf->pFile->namelist[TmpN]->d_type)
            //                                        {
            //                                            // Make room room for ending "/"
            //                                            Len++;
            //                                        }
            //                                        else
            //                                        {
            //                                            // Make room for md5sum + space + Filelength (fixed to 8 chars) + space
            //                                            Len += MD5LEN + 1 + SIZEOFFILESIZE + 1;
            //                                        }

            //                                        // Add one new line character per file/folder/link -name
            //                                        Len++;
            //                                    }
            //                                }

            //                                pTxBuf->pFile->Size = Len;

            //                                // Total list length has been calculated
            //                                pReplyBeginList->ListSizeLsb = (UBYTE)Len;
            //                                pReplyBeginList->ListSizeNsb1 = (UBYTE)(Len >> 8);
            //                                pReplyBeginList->ListSizeNsb2 = (UBYTE)(Len >> 16);
            //                                pReplyBeginList->ListSizeMsb = (UBYTE)(Len >> 24);

            //                                pTxBuf->MsgLen = BytesToRead;
            //                                pTxBuf->SendBytes = 0;
            //                                pTxBuf->pFile->Pointer = 0;
            //                                pTxBuf->pFile->Length = 0;

            //                                if (BytesToRead > Len)
            //                                {
            //                                    // if message length  > file size then crop message
            //                                    pTxBuf->MsgLen = Len;
            //                                    pReplyBeginList->Status = END_OF_FILE; // Complete file in first message
            //                                }

            //                                TmpN = pTxBuf->pFile->File;          // Make a copy of number of entries

            //                                if ((pTxBuf->MsgLen + SIZEOF_RPLYBEGINLIST) <= pTxBuf->BufSize)
            //                                {
            //                                    //All requested bytes can be inside the buffer
            //                                    BytesToSend = pTxBuf->MsgLen;
            //                                    pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYBEGINLIST;
            //                                    pTxBuf->State = TXIDLE;
            //                                }
            //                                else
            //                                {
            //                                    BytesToSend = (pTxBuf->BufSize - SIZEOF_RPLYBEGINLIST);
            //                                    pTxBuf->BlockLen = pTxBuf->BufSize;
            //                                    pTxBuf->State = TXLISTFILES;
            //                                }

            //                                Repeat = 1;
            //                                Len = 0;
            //                                pDstAdr = pReplyBeginList->PayLoad;
            //                                while (Repeat)
            //                                {
            //                                    TmpN--;

            //                                    cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (char*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

            //                                    if (0 != NameLen)
            //                                    {
            //                                        if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
            //                                        {

            //                                            pSrcAdr = (UBYTE*)(TmpFileName);
            //                                            while (*pSrcAdr) *pDstAdr++ = *pSrcAdr++;
            //                                            Len += NameLen;

            //                                            free(pTxBuf->pFile->namelist[TmpN]);

            //                                            if (BytesToSend == Len)
            //                                            {
            //                                                // buffer is filled up now exit
            //                                                pTxBuf->pFile->Length = 0;
            //                                                pTxBuf->pFile->File = TmpN;
            //                                                Repeat = 0;
            //                                            }
            //                                        }
            //                                        else
            //                                        {
            //                                            // No - Now fill up to exact size
            //                                            pTxBuf->pFile->Length = (BytesToSend - Len);
            //                                            memcpy((char*)pDstAdr, TmpFileName, pTxBuf->pFile->Length);
            //                                            Len += pTxBuf->pFile->Length;
            //                                            pTxBuf->pFile->File = TmpN;                         // Adjust Iteration number
            //                                            Repeat = 0;
            //                                        }
            //                                    }
            //                                }

            //                                // Update pointers
            //                                pTxBuf->SendBytes = Len;
            //                                pTxBuf->pFile->Pointer = Len;

            //                                if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
            //                                {
            //# ifdef DEBUG
            //                                    printf("Complete list of %lu Bytes uploaded \r\n", (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                                    pReplyBeginList->Status = END_OF_FILE;
            //                                    cComFreeHandle(FileHandle);
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            // No more handle or illegal file name
            //                            pReplyBeginList->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyBeginList->Status = UNKNOWN_ERROR;
            //                            pReplyBeginList->ListSizeLsb = 0;
            //                            pReplyBeginList->ListSizeNsb1 = 0;
            //                            pReplyBeginList->ListSizeNsb2 = 0;
            //                            pReplyBeginList->ListSizeMsb = 0;
            //                            pReplyBeginList->Handle = -1;

            //                            pTxBuf->BlockLen = SIZEOF_RPLYBEGINLIST;
            //                        }
            //                        pReplyBeginList->CmdSize += pTxBuf->MsgLen;

            //# ifdef DEBUG
            //                        cComPrintTxMsg(pTxBuf);
            //#endif

            //                    }
            //                    break;

            //                case CONTINUE_LIST_FILES:
            //                    {
            //                        SLONG TmpN;
            //                        ULONG BytesToRead;
            //                        ULONG Len;
            //                        ULONG BytesToSend;
            //                        ULONG NameLen;
            //                        ULONG RemCharCnt;
            //                        UBYTE Repeat;
            //                        char TmpFileName[FILENAMESIZE];

            //                        CONTINUE_LIST* pContinueList;
            //                        RPLY_CONTINUE_LIST* pReplyContinueList;

            //                        //Setup pointers
            //                        pContinueList = (CONTINUE_LIST*)pRxBuf->Buf;
            //                        pReplyContinueList = (RPLY_CONTINUE_LIST*)pTxBuf->Buf;

            //                        pReplyContinueList->CmdSize = SIZEOF_RPLYCONTINUELIST - sizeof(CMDSIZE);
            //                        pReplyContinueList->MsgCount = pContinueList->MsgCount;
            //                        pReplyContinueList->CmdType = SYSTEM_REPLY;
            //                        pReplyContinueList->Cmd = CONTINUE_LIST_FILES;
            //                        pReplyContinueList->Handle = pContinueList->Handle;
            //                        pReplyContinueList->Status = SUCCESS;

            //                        BytesToRead = (ULONG)pContinueList->BytesToReadLsb;
            //                        BytesToRead += (ULONG)pContinueList->BytesToReadMsb << 8;

            //                        if (((pTxBuf->pFile->File == 0) && (0 > pTxBuf->pFile->Length)) || (pTxBuf->pFile->File < 0))
            //                        {
            //                            // here if nothing is to be returned
            //                            pReplyContinueList->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyContinueList->Status = UNKNOWN_ERROR;
            //                            pReplyContinueList->Handle = -1;

            //                            pTxBuf->MsgLen = 0;
            //                            pTxBuf->BlockLen = SIZEOF_RPLYCONTINUELIST;
            //                            cComFreeHandle(FileHandle);
            //                        }
            //                        else
            //                        {
            //                            TmpN = pTxBuf->pFile->File;       // Make a copy of number of entries

            //                            pTxBuf->MsgLen = BytesToRead;
            //                            pTxBuf->SendBytes = 0;

            //                            if (BytesToRead >= (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer))
            //                            {
            //                                // if message length  > file size then crop message
            //                                pTxBuf->MsgLen = (pTxBuf->pFile->Size - pTxBuf->pFile->Pointer);
            //                                pReplyContinueList->Status = END_OF_FILE;           // Remaining file included in this message
            //                            }

            //                            if ((pTxBuf->MsgLen + SIZEOF_RPLYCONTINUELIST) <= pTxBuf->BufSize)
            //                            {
            //                                BytesToSend = pTxBuf->MsgLen;
            //                                pTxBuf->BlockLen = pTxBuf->MsgLen + SIZEOF_RPLYCONTINUELIST;
            //                                pTxBuf->State = TXIDLE;
            //                            }
            //                            else
            //                            {
            //                                BytesToSend = pTxBuf->BufSize - SIZEOF_RPLYCONTINUELIST;
            //                                pTxBuf->BlockLen = pTxBuf->BufSize;
            //                                pTxBuf->State = TXLISTFILES;
            //                            }

            //                            Len = 0;
            //                            Repeat = 1;
            //                            if (pTxBuf->pFile->Length)
            //                            {

            //                                // Only here if filename has been divided in 2 pieces
            //                                cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (char*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

            //                                if (0 != NameLen)
            //                                {
            //                                    // First transfer the remaining part of the last filename
            //                                    RemCharCnt = NameLen - pTxBuf->pFile->Length;

            //                                    if (RemCharCnt <= BytesToSend)
            //                                    {
            //                                        // this will fit into the message length
            //                                        memcpy((char*)(&(pReplyContinueList->PayLoad[Len])), &(TmpFileName[pTxBuf->pFile->Length]), RemCharCnt);
            //                                        Len += RemCharCnt;

            //                                        free(pTxBuf->pFile->namelist[TmpN]);

            //                                        if (RemCharCnt == BytesToSend)
            //                                        {
            //                                            //if If all bytes is already occupied not more to go
            //                                            //for this message
            //                                            Repeat = 0;
            //                                            pTxBuf->pFile->Length = 0;
            //                                        }
            //                                        else
            //                                        {
            //                                            Repeat = 1;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        // This is the rare condition if remaining msg len and buf size are almost equal
            //                                        memcpy((char*)(&(pReplyContinueList->PayLoad[Len])), &(TmpFileName[pTxBuf->pFile->Length]), BytesToSend);
            //                                        Len += BytesToSend;

            //                                        pTxBuf->pFile->File = TmpN;                         // Adjust Iteration number
            //                                        pTxBuf->pFile->Length += Len;
            //                                        Repeat = 0;
            //                                    }
            //                                }
            //                            }
            //                            if (TmpN)
            //                            {
            //                                while (Repeat)
            //                                {
            //                                    TmpN--;

            //                                    cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (char*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

            //                                    if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
            //                                    {
            //                                        memcpy((char*)(&(pReplyContinueList->PayLoad[Len])), TmpFileName, NameLen);
            //                                        Len += NameLen;

            //                                        free(pTxBuf->pFile->namelist[TmpN]);

            //                                        if (BytesToSend == Len)
            //                                        {
            //                                            // buffer is filled up now exit
            //                                            pTxBuf->pFile->Length = 0;
            //                                            pTxBuf->pFile->File = TmpN;
            //                                            Repeat = 0;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        // Now fill up to complete buffer size
            //                                        pTxBuf->pFile->Length = (BytesToSend - Len);
            //                                        memcpy((char*)(&(pReplyContinueList->PayLoad[Len])), TmpFileName, pTxBuf->pFile->Length);
            //                                        Len += pTxBuf->pFile->Length;
            //                                        pTxBuf->pFile->File = TmpN;         // Adjust Iteration number
            //                                        Repeat = 0;
            //                                    }
            //                                }
            //                            }
            //                        }

            //                        // Update pointers
            //                        pTxBuf->SendBytes = Len;
            //                        pTxBuf->pFile->Pointer += Len;

            //                        if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
            //                        {
            //# ifdef DEBUG
            //                            printf("Complete list of %lu Bytes uploaded \r\n", (unsigned long)pTxBuf->pFile->Length);
            //#endif

            //                            pReplyContinueList->Status = END_OF_FILE;
            //                            cComFreeHandle(pContinueList->Handle);
            //                        }

            //                        pReplyContinueList->CmdSize += pTxBuf->MsgLen;

            //# ifdef DEBUG
            //                        cComPrintTxMsg(pTxBuf);
            //#endif
            //                    }
            //                    break;

            //                case CLOSE_FILEHANDLE:
            //                    {
            //                        CLOSE_HANDLE* pCloseHandle;
            //                        RPLY_CLOSE_HANDLE* pReplyCloseHandle;

            //                        //Setup pointers
            //                        pCloseHandle = (CLOSE_HANDLE*)pRxBuf->Buf;
            //                        pReplyCloseHandle = (RPLY_CLOSE_HANDLE*)pTxBuf->Buf;

            //                        FileHandle = pCloseHandle->Handle;

            //# ifdef DEBUG
            //                        printf("FileHandle to close = %d, Linux Handle = %d\r\n", FileHandle, ComInstance.Files[FileHandle].File);
            //#endif

            //                        pReplyCloseHandle->CmdSize = SIZEOF_RPLYCLOSEHANDLE - sizeof(CMDSIZE);
            //                        pReplyCloseHandle->MsgCount = pCloseHandle->MsgCount;
            //                        pReplyCloseHandle->CmdType = SYSTEM_REPLY;
            //                        pReplyCloseHandle->Cmd = CLOSE_FILEHANDLE;
            //                        pReplyCloseHandle->Handle = pCloseHandle->Handle;
            //                        pReplyCloseHandle->Status = SUCCESS;

            //                        if (TRUE == cComFreeHandle(FileHandle))
            //                        {
            //                            cComCloseFileHandle(&(ComInstance.Files[FileHandle].File));
            //                        }
            //                        else
            //                        {
            //                            pReplyCloseHandle->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyCloseHandle->Status = UNKNOWN_HANDLE;
            //                        }
            //                        pTxBuf->BlockLen = SIZEOF_RPLYCLOSEHANDLE;

            //# ifdef DEBUG
            //                        cComPrintTxMsg(pTxBuf);
            //#endif

            //                    }
            //                    break;

            //                case CREATE_DIR:
            //                    {
            //                        MAKE_DIR* pMakeDir;
            //                        RPLY_MAKE_DIR* pReplyMakeDir;
            //                        char Folder[sizeof(ComInstance.Files[FileHandle].Name)];

            //                        //Setup pointers
            //                        pMakeDir = (MAKE_DIR*)pRxBuf->Buf;
            //                        pReplyMakeDir = (RPLY_MAKE_DIR*)pTxBuf->Buf;

            //                        pReplyMakeDir->CmdSize = SIZEOF_RPLYMAKEDIR - sizeof(CMDSIZE);
            //                        pReplyMakeDir->MsgCount = pMakeDir->MsgCount;
            //                        pReplyMakeDir->CmdType = SYSTEM_REPLY;
            //                        pReplyMakeDir->Cmd = CREATE_DIR;
            //                        pReplyMakeDir->Status = SUCCESS;

            //                        snprintf(Folder, sizeof(Folder), "%s", (char*)(pMakeDir->Dir));

            //                        if (0 == mkdir(Folder, S_IRWXU | S_IRWXG | S_IRWXO))
            //                        {
            //                            chmod(Folder, S_IRWXU | S_IRWXG | S_IRWXO);
            //# ifdef DEBUG
            //                            printf("Folder %s created\r\n", Folder);
            //#endif
            //                            SetUiUpdate();
            //                        }
            //                        else
            //                        {
            //                            pReplyMakeDir->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyMakeDir->Status = NO_PERMISSION;

            //# ifdef DEBUG
            //                            printf("Folder %s not created (%s)\r\n", Folder, strerror(errno));
            //#endif
            //                        }
            //                        pTxBuf->BlockLen = SIZEOF_RPLYMAKEDIR;
            //                    }
            //                    break;

            //                case DELETE_FILE:
            //                    {
            //                        REMOVE_FILE* pRemove;
            //                        RPLY_REMOVE_FILE* pReplyRemove;
            //                        char Name[60];

            //                        //Setup pointers
            //                        pRemove = (REMOVE_FILE*)pRxBuf->Buf;
            //                        pReplyRemove = (RPLY_REMOVE_FILE*)pTxBuf->Buf;

            //                        pReplyRemove->CmdSize = SIZEOF_RPLYREMOVEFILE - sizeof(CMDSIZE);
            //                        pReplyRemove->MsgCount = pRemove->MsgCount;
            //                        pReplyRemove->CmdType = SYSTEM_REPLY;
            //                        pReplyRemove->Cmd = DELETE_FILE;
            //                        pReplyRemove->Status = SUCCESS;

            //                        snprintf(Name, 60, "%s", (char*)(pRemove->Name));

            //# ifdef DEBUG
            //                        printf("File to delete %s\r\n", Name);
            //#endif

            //                        if (0 == remove(Name))
            //                        {
            //                            SetUiUpdate();
            //                        }
            //                        else
            //                        {
            //                            pReplyRemove->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyRemove->Status = NO_PERMISSION;

            //# ifdef DEBUG
            //                            printf("Folder %s not deleted (%s)\r\n", Folder, strerror(errno));
            //#endif
            //                        }
            //                        pTxBuf->BlockLen = SIZEOF_RPLYREMOVEFILE;
            //                    }
            //                    break;

            //                case LIST_OPEN_HANDLES:
            //                    {
            //                        UBYTE HCnt1, HCnt2;

            //                        LIST_HANDLES* pListHandles;
            //                        RPLY_LIST_HANDLES* pReplyListHandles;

            //                        //Setup pointers
            //                        pListHandles = (LIST_HANDLES*)pRxBuf->Buf;
            //                        pReplyListHandles = (RPLY_LIST_HANDLES*)pTxBuf->Buf;

            //                        pReplyListHandles->CmdSize = SIZEOF_RPLYLISTHANDLES - sizeof(CMDSIZE);
            //                        pReplyListHandles->MsgCount = pListHandles->MsgCount;
            //                        pReplyListHandles->CmdType = SYSTEM_REPLY;
            //                        pReplyListHandles->Cmd = LIST_OPEN_HANDLES;
            //                        pReplyListHandles->Status = SUCCESS;

            //                        for (HCnt1 = 0; HCnt1 < ((MAX_FILE_HANDLES / 8) + 1); HCnt1++)
            //                        {

            //                            pReplyListHandles->PayLoad[HCnt1 + 2] = 0;

            //                            for (HCnt2 = 0; HCnt2 < 8; HCnt2++)
            //                            {

            //                                if (0 != ComInstance.Files[HCnt2 * HCnt1].State)
            //                                { // Filehandle is in use

            //                                    pReplyListHandles->PayLoad[HCnt1 + 2] |= (0x01 << HCnt2);
            //                                }
            //                            }
            //                        }
            //                        pReplyListHandles->CmdSize += HCnt1;
            //                        pTxBuf->BlockLen = SIZEOF_RPLYLISTHANDLES + HCnt1;
            //                    }
            //                    break;

            //                case WRITEMAILBOX:
            //                    {
            //                        UBYTE No;
            //                        UWORD PayloadSize;
            //                        WRITE_MAILBOX* pWriteMailbox;
            //                        WRITE_MAILBOX_PAYLOAD* pWriteMailboxPayload;

            //                        pWriteMailbox = (WRITE_MAILBOX*)pRxBuf->Buf;

            //                        if (1 == cComFindMailbox(&(pWriteMailbox->Name[0]), &No))
            //                        {
            //                            pWriteMailboxPayload = (WRITE_MAILBOX_PAYLOAD*)&(pWriteMailbox->Name[(pWriteMailbox->NameSize)]);
            //                            PayloadSize = (UWORD)(pWriteMailboxPayload->SizeLsb);
            //                            PayloadSize += ((UWORD)(pWriteMailboxPayload->SizeMsb)) << 8;
            //                            memcpy(ComInstance.MailBox[No].Content, pWriteMailboxPayload->Payload, PayloadSize);
            //                            ComInstance.MailBox[No].DataSize = PayloadSize;
            //                            ComInstance.MailBox[No].WriteCnt++;
            //                        }
            //                    }
            //                    break;

            //                case BLUETOOTHPIN:
            //                    {
            //                        // Both MAC and Pin are zero terminated string type
            //                        UBYTE BtAddr[vmBTADRSIZE];
            //                        UBYTE Pin[vmBTPASSKEYSIZE];
            //                        UBYTE PinSize;
            //                        BLUETOOTH_PIN* pBtPin;
            //                        RPLY_BLUETOOTH_PIN* pReplyBtPin;

            //                        pBtPin = (BLUETOOTH_PIN*)pRxBuf->Buf;
            //                        pReplyBtPin = (RPLY_BLUETOOTH_PIN*)pTxBuf->Buf;

            //                        snprintf((char*)BtAddr, pBtPin->MacSize, "%s", (char*)pBtPin->Mac);
            //                        PinSize = pBtPin->PinSize;
            //                        snprintf((char*)Pin, PinSize, "%s", (char*)pBtPin->Pin);

            //                        // This command can for safety reasons only be handled by USB
            //                        if (USBDEV == ComInstance.ActiveComCh)
            //                        {
            //                            cBtSetTrustedDev(BtAddr, Pin, PinSize);
            //                            pReplyBtPin->Status = SUCCESS;
            //                        }
            //                        else
            //                        {
            //                            pReplyBtPin->Status = ILLEGAL_CONNECTION;
            //                        }

            //                        pReplyBtPin->CmdSize = 0x00;
            //                        pReplyBtPin->MsgCount = pBtPin->MsgCount;
            //                        pReplyBtPin->CmdType = SYSTEM_REPLY;
            //                        pReplyBtPin->Cmd = BLUETOOTHPIN;
            //                        pReplyBtPin->MacSize = vmBTADRSIZE;

            //                        cBtGetId(pReplyBtPin->Mac, vmBTADRSIZE);
            //                        pReplyBtPin->PinSize = PinSize;
            //                        memcpy(pReplyBtPin->Pin, pBtPin->Pin, PinSize);

            //                        pReplyBtPin->CmdSize = (SIZEOF_RPLYBLUETOOTHPIN + pReplyBtPin->MacSize + pReplyBtPin->PinSize + sizeof(pReplyBtPin->MacSize) + sizeof(pReplyBtPin->PinSize)) -sizeof(CMDSIZE);
            //                        pTxBuf->BlockLen = (pReplyBtPin->CmdSize) + sizeof(CMDSIZE);
            //                    }
            //                    break;

            //                case ENTERFWUPDATE:
            //                    {
            //                        ULONG UpdateFile;
            //                        UBYTE Dummy;

            //                        if (USBDEV == ComInstance.ActiveComCh)
            //                        {
            //                            UpdateFile = open(UPDATE_DEVICE_NAME, O_RDWR);

            //                            if (UpdateFile >= 0)
            //                            {
            //                                write(UpdateFile, &Dummy, 1);
            //                                close(UpdateFile);
            //                                system("reboot -d -f -i");
            //                            }
            //                        }
            //                    }
            //                    break;

            //                case SETBUNDLEID:
            //                    {
            //                        BUNDLE_ID* pBundleId;
            //                        RPLY_BUNDLE_ID* pReplyBundleId;

            //                        pBundleId = (BUNDLE_ID*)pRxBuf->Buf;
            //                        pReplyBundleId = (RPLY_BUNDLE_ID*)pTxBuf->Buf;

            //                        pReplyBundleId->CmdSize = 0x05;
            //                        pReplyBundleId->MsgCount = pBundleId->MsgCount;
            //                        pReplyBundleId->Cmd = SETBUNDLEID;

            //                        if (TRUE == cBtSetBundleId(pBundleId->BundleId))
            //                        {
            //                            // Success
            //                            pReplyBundleId->CmdType = SYSTEM_REPLY;
            //                            pReplyBundleId->Status = SUCCESS;
            //                        }
            //                        else
            //                        {
            //                            // Error
            //                            pReplyBundleId->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyBundleId->Status = SIZE_ERROR;
            //                        }
            //                        pTxBuf->BlockLen = SIZEOF_RPLYBUNDLEID;
            //                    }
            //                    break;

            //                case SETBUNDLESEEDID:
            //                    {
            //                        BUNDLE_SEED_ID* pBundleSeedId;
            //                        RPLY_BUNDLE_SEED_ID* pReplyBundleSeedId;

            //                        pBundleSeedId = (BUNDLE_SEED_ID*)pRxBuf->Buf;
            //                        pReplyBundleSeedId = (RPLY_BUNDLE_SEED_ID*)pTxBuf->Buf;

            //                        pReplyBundleSeedId->CmdSize = 0x05;
            //                        pReplyBundleSeedId->MsgCount = pBundleSeedId->MsgCount;
            //                        pReplyBundleSeedId->Cmd = SETBUNDLESEEDID;

            //                        if (TRUE == cBtSetBundleSeedId(pBundleSeedId->BundleSeedId))
            //                        {
            //                            // Success
            //                            pReplyBundleSeedId->CmdType = SYSTEM_REPLY;
            //                            pReplyBundleSeedId->Status = SUCCESS;
            //                        }
            //                        else
            //                        {
            //                            // Error
            //                            pReplyBundleSeedId->CmdType = SYSTEM_REPLY_ERROR;
            //                            pReplyBundleSeedId->Status = SIZE_ERROR;
            //                        }
            //                        pTxBuf->BlockLen = SIZEOF_RPLYBUNDLESEEDID;
            //                    }
            //                    break;
            //            }
        }

        public void cComUpdate()
        {
            COMCMD pComCmd;
            IMGHEAD pImgHead;
            TXBUF pTxBuf;
            RXBUF pRxBuf;
            UBYTE ChNo;
            UWORD BytesRead;

            UBYTE ThisMagicCookie;
            UBYTE HelperByte;
            uint Iterator;
            UBYTE MotorBusySignal;
            UBYTE MotorBusySignalPointer;

            ChNo = 0;

            // TODO: COM UPDATE
            //            for (ChNo = 0; ChNo < NO_OF_CHS; ChNo++)
            //            {

            //                pTxBuf = &(ComInstance.TxBuf[ChNo]);
            //                pRxBuf = &(ComInstance.RxBuf[ChNo]);
            //                BytesRead = 0;

            //                if (!pTxBuf->Writing)
            //                {
            //                    if (NULL != ComInstance.ReadChannel[ChNo])
            //                    {
            //                        if (ComInstance.VmReady == 1)
            //                        {
            //                            BytesRead = ComInstance.ReadChannel[ChNo](pRxBuf->Buf, pRxBuf->BufSize);
            //                        }
            //                    }

            //#undef DEBUG
            //                    //#define DEBUG
            //# ifdef DEBUG
            //                    // start DEBUG
            //                    if (ChNo == USBDEV)
            //                    {
            //                        printf("Writing NOT set @ USBDEV - BytesRead = %d\r\n", BytesRead);
            //                    }
            //                    // end DEBUG
            //#endif


            //                    if (BytesRead)
            //                    {
            //                        // Temporary fix until full implementation of com channels is ready
            //                        ComInstance.ActiveComCh = ChNo;

            //# ifdef DEBUG
            //                        //          cComShow(ComInstance.UsbCmdInRep);
            //#endif

            //                        if (RXIDLE == pRxBuf->State)
            //                        {
            //                            // Not file down-loading
            //                            memset(pTxBuf->Buf, 0, sizeof(pTxBuf->Buf));

            //            pComCmd = (COMCMD*)pRxBuf->Buf;

            //            if ((*pComCmd).CmdSize)
            //            {
            //                // message received
            //                switch ((*pComCmd).Cmd)
            //                {
            //                    // NEW MOTOR/DAISY
            //                    case DIR_CMD_NO_REPLY_WITH_BUSY:
            //                        {
            //#undef DEBUG
            //                            //#define DEBUG
            //# ifdef DEBUG
            //                            printf("Did we reach a *BUSY* DIRECT_COMMAND_NO_REPLY pComCmd.Size = %d\n\r", ((*pComCmd).CmdSize));
            //#endif

            //                            Iterator = ((*pComCmd).CmdSize) - 7; // Incl. LEN bytes

            //                            HelperByte = 0x00;

            //                            for (Iterator = (((*pComCmd).CmdSize) - 7); Iterator < (((*pComCmd).CmdSize) - 3); Iterator++)
            //                            {
            //                                HelperByte |= ((*pComCmd).PayLoad[Iterator] & 0x03);
            //                            }

            //                            cDaisySetOwnLayer(HelperByte);

            //                            // Setup the Cookie stuff get one by one and store

            //                            HelperByte = 0; // used as Index

            //                            // New MotorSignal
            //                            MotorBusySignal = 0;
            //                            MotorBusySignalPointer = 1;

            //                            for (Iterator = (((*pComCmd).CmdSize) - 7); Iterator < (((*pComCmd).CmdSize) - 3); Iterator++)
            //                            {
            //#undef DEBUG
            //                                //#define DEBUG
            //# ifdef DEBUG
            //                                printf("Iterator = %d\n\r", Iterator);
            //#endif

            //                                ThisMagicCookie = (*pComCmd).PayLoad[Iterator];

            //                                // New MotorSignal
            //                                if (ThisMagicCookie & 0x80) // BUSY signalled
            //                                {
            //                                    MotorBusySignal = (UBYTE)(MotorBusySignal | MotorBusySignalPointer);
            //                                }

            //                                // New MotorSignal
            //                                MotorBusySignalPointer <<= 1;

            //#undef DEBUG
            //                                //#define DEBUG
            //# ifdef DEBUG
            //                                printf("ThisMagicCookie = %d\n\r", ThisMagicCookie);
            //#endif

            //                                cDaisySetBusyFlags(cDaisyGetOwnLayer(), HelperByte, ThisMagicCookie);
            //                                HelperByte++;
            //                            }

            //                            ResetDelayCounter(MotorBusySignal);

            //#undef DEBUG
            //                            //#define DEBUG
            //# ifdef DEBUG
            //                            printf("cMotorSetBusyFlags(%X)\n\r", MotorBusySignal);
            //#endif

            //                            // New MotorSignal
            //                            cMotorSetBusyFlags(MotorBusySignal);

            //                            // Adjust length

            //                            ((*pComCmd).CmdSize) -= 4;  // NO VM use of cookies (or sweets ;-))

            //                            // Now as "normal" direct command with NO reply

            //                            ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
            //                        }
            //                        break;

            //                    case DIRECT_COMMAND_REPLY:
            //                        {
            //                            // direct command

            //#undef DEBUG
            //                            //#define DEBUG
            //# ifdef DEBUG
            //                            printf("Did we reach a DIRECT_COMMAND_REPLY\n\r");
            //#endif

            //                            if (0 == ComInstance.ReplyStatus)
            //                            {
            //                                // If ReplyStstus = 0 then no commands is currently being
            //                                // processed -> new command can be processed
            //                                ComInstance.ReplyStatus |= DIR_CMD_REPLY;
            //                                ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
            //                                if (!ComInstance.CommandReady)
            //                                {
            //                                    // some error
            //                                    pTxBuf->Writing = 1;
            //                                    ComInstance.ReplyStatus = 0;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                // Else VM is currently processing direct commands
            //                                // send error command, only if a DIR_CMD_NOREPLY
            //                                // is being executed. Not if a DIR_CMD_NOREPLY is
            //                                // being executed as replies can collide.
            //                                if (DIR_CMD_NOREPLY & ComInstance.ReplyStatus)
            //                                {
            //                                    COMCMD* pComCmd;
            //                                    COMRPL* pComRpl;

            //                                    pComCmd = (COMCMD*)pRxBuf->Buf;
            //                                    pComRpl = (COMRPL*)pTxBuf->Buf;

            //                                    (*pComRpl).CmdSize = 3;
            //                                    (*pComRpl).MsgCnt = (*pComCmd).MsgCnt;
            //                                    (*pComRpl).Cmd = DIRECT_REPLY_ERROR;

            //                                    pTxBuf->Writing = 1;
            //                                }
            //                            }
            //                        }
            //                        break;

            //                    case DIRECT_COMMAND_NO_REPLY:
            //                        {
            //                            // direct command

            //#undef DEBUG
            //                            //#define DEBUG
            //# ifdef DEBUG
            //                            printf("Did we reach a DIRECT_COMMAND_NO_REPLY\n\r");
            //#endif

            //                            //Do not reply even if error
            //                            if (0 == ComInstance.ReplyStatus)
            //                            {
            //                                // If ReplyStstus = 0 then no commands is currently being
            //                                // processed -> new command can be processed
            //                                ComInstance.ReplyStatus |= DIR_CMD_NOREPLY;
            //                                ComInstance.CommandReady = cComDirectCommand(pRxBuf->Buf, pTxBuf->Buf);
            //                            }
            //                        }
            //                        break;

            //                    case SYSTEM_COMMAND_REPLY:
            //                        {
            //                            if (0 == ComInstance.ReplyStatus)
            //                            {
            //                                ComInstance.ReplyStatus |= SYS_CMD_REPLY;
            //                                cComSystemCommand(pRxBuf, pTxBuf);
            //                                if (RXFILEDL != pRxBuf->State)
            //                                {
            //                                    // Only here if command has been completed
            //                                    pTxBuf->Writing = 1;
            //                                    ComInstance.ReplyStatus = 0;
            //                                }
            //                            }
            //                        }
            //                        break;

            //                    case SYSTEM_COMMAND_NO_REPLY:
            //                        {
            //                            if (0 == ComInstance.ReplyStatus)
            //                            {
            //                                ComInstance.ReplyStatus |= SYS_CMD_NOREPLY;
            //                                cComSystemCommand(pRxBuf, pTxBuf);
            //                                if (RXFILEDL != pRxBuf->State)
            //                                {
            //                                    // Only here if command has been completed
            //                                    ComInstance.ReplyStatus = 0;
            //                                }
            //                            }
            //                        }
            //                        break;

            //                    case SYSTEM_REPLY:
            //                        {
            //                            cComSystemReply(pRxBuf, pTxBuf);
            //                        }
            //                        break;

            //                    case SYSTEM_REPLY_ERROR:
            //                        {
            //# ifdef DEBUG
            //                            printf("\r\nsystem reply error\r\n");
            //#endif
            //                        }
            //                        break;

            //                    case DAISY_COMMAND_REPLY:
            //                        {
            //                            if (ChNo == USBDEV)
            //                            {
            //#undef DEBUG
            //                                //#define DEBUG
            //# ifdef DEBUG
            //                                printf("Did we reach c_COM @ DAISY_COMMAND_REPLY?\n\r");
            //#endif

            //                                cDaisyCmd(pRxBuf, pTxBuf);

            //                            }
            //                            else
            //                            {
            //                                // Some ERROR handling
            //                            }
            //                        }
            //                        break;

            //                    case DAISY_COMMAND_NO_REPLY:
            //                        {

            //#undef DEBUG
            //                            //#define DEBUG
            //# ifdef DEBUG
            //                            printf("Did we reach c_COM @ DAISY_COMMAND_NO_REPLY?\n\r");
            //#endif

            //                            // A Daisy command without any reply
            //                            if (ChNo == USBDEV)
            //                            {
            //                                // Do something
            //                                cDaisyCmd(pRxBuf, pTxBuf);
            //                            }
            //                            else
            //                            {
            //                                // Some ERROR handling
            //                            }
            //                        }
            //                        break;

            //                    default:
            //                        {
            //                        }
            //                        break;

            //                }
            //            }
            //            else
            //            { // poll received
            //              // send response

            //                pImgHead = (IMGHEAD*)ComInstance.Image;
            //                pComCmd = (COMCMD*)pTxBuf->Buf;

            //                (*pComCmd).CmdSize = (CMDSIZE)(*pImgHead).GlobalBytes + 1;
            //                (*pComCmd).Cmd = DIRECT_REPLY;
            //                memcpy((*pComCmd).PayLoad, ComInstance.Globals, (*pImgHead).GlobalBytes);

            //                pTxBuf->Writing = 1;
            //            }
            //        }
            //        else
            //        { // in the middle of a write file command
            //          ULONG RemBytes;
            //        ULONG BytesToWrite;

            //        RemBytes = pRxBuf->MsgLen - pRxBuf->RxBytes;

            //          if (RemBytes <= pRxBuf->BufSize)
            //          {
            //            // Remaining bytes to write
            //            BytesToWrite      =  RemBytes;

            //            // Send the reply if requested
            //            if (ComInstance.ReplyStatus  &  SYS_CMD_REPLY)
            //            {
            //              pTxBuf->Writing   =  1;
            //            }

            //    // Clear to receive next msg header
            //    pRxBuf->State            =  RXIDLE;
            //            ComInstance.ReplyStatus  =  0;
            //          }
            //          else
            //{
            //    BytesToWrite = pRxBuf->BufSize;
            //}

            //write(pRxBuf->pFile->File, pRxBuf->Buf, (size_t)BytesToWrite);
            //pRxBuf->pFile->Pointer += (ULONG)BytesToWrite;
            //pRxBuf->RxBytes += (ULONG)BytesToWrite;

            //if (pRxBuf->pFile->Pointer >= pRxBuf->pFile->Size)
            //{
            //    cComCloseFileHandle(&(pRxBuf->pFile->File));
            //    chmod(pRxBuf->pFile->Name, S_IRWXU | S_IRWXG | S_IRWXO);
            //    cComFreeHandle(pRxBuf->FileHandle);
            //}
            //        }
            //      }
            //    }
            //    cComTxUpdate(ChNo);

            //// Time for USB unplug detect?
            //UsbConUpdate++;
            //if (UsbConUpdate >= USB_CABLE_DETECT_RATE)
            //{
            //    //#define DEBUG
            //# ifdef DEBUG
            //    printf("ready to check\n\r");
            //#endif
            //    UsbConUpdate = 0;
            //    cComUsbDeviceConnected = cComCheckUsbCable();
            //}

            //  }

            //  BtUpdate();
            //cWiFiControl();
        }

        public void cComTxUpdate(byte ChNo)
        {
            TXBUF pTxBuf;
            ULONG ReadBytes;

            // TODO: TX UPDATE

//            pTxBuf = &(ComInstance.TxBuf[ChNo]);

//            if ((OK == cDaisyChained()) && (ChNo == USBDEV)) // We're part of a chain && USBDEV
//            {
//                // Do "special handling" - I.e. no conflict between pushed DaisyData and non-syncronized
//                // returns from Commands (answers/errors).

//                //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                printf("\n\r001\n\r");
//#endif

//                if (GetDaisyPushCounter() == DAISY_PUSH_NOT_UNLOCKED)  // It's the very first
//                {                                                     // (or one of the first ;-)) transmission(s)
//                                                                      //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                    printf("Not unlocked 001\n\r");
//#endif

//                    if (pTxBuf->Writing)  // Anything Pending?
//                    {
//                        //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                        printf("Not unlocked 002\n\r");
//#endif

//                        if (NULL != ComInstance.WriteChannel[ChNo])  // Valid channel?
//                        {
//                            //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                            printf("Not unlocked 003\n\r");
//#endif

//                            if ((ComInstance.WriteChannel[USBDEV](pTxBuf->Buf, pTxBuf->BlockLen)) != 0)
//                            {
//                                //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                                printf("Not (OR should be) unlocked 004\n\r");
//#endif

//                                pTxBuf->Writing = 0;

//                                ResetDaisyPushCounter();  // Ready for normal run

//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    // We're unlocked

//                    if (GetDaisyPushCounter() == 0)  // It's a NON DaisyChain time-slice
//                    {
//                        //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                        printf("Unlocked 001\n\r");
//#endif

//                        if (pTxBuf->Writing)
//                        {
//                            //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                            printf("Unlocked 002\n\r");
//#endif

//                            if (NULL != ComInstance.WriteChannel[ChNo])
//                            {
//                                //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                                printf("Unlocked 003\n\r");
//#endif

//                                if (ComInstance.WriteChannel[ChNo](pTxBuf->Buf, pTxBuf->BlockLen))
//                                {
//                                    //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                                    printf("Unlocked 004\n\r");
//#endif

//                                    pTxBuf->Writing = 0;
//                                    ResetDaisyPushCounter();  // Done/or we'll wait - we can allow more Daisy stuff
//                                }

//                            }
//                        }
//                        else
//                        {
//                            //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                            printf("Unlocked 005\n\r");
//#endif

//                            ResetDaisyPushCounter();      // Skip this "master" slice - use time with more benefit ;-)
//                        }
//                    }
//                    else
//                    {
//                        // We have a prioritised Daisy transfer/time-slice
//                        // DaisyPushCounter == either 3, 2 or 1

//                        if (NULL != ComInstance.WriteChannel[USBDEV])
//                        {
//                            UBYTE* pData;     // Pointer to dedicated Daisy Upstream Buffer (INFO or Data)
//                            UWORD Len = 0;

//                            Len = cDaisyData(&pData);

//                            //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                            printf("Daisy Len = %d, Counter = %d\n\r", Len, GetDaisyPushCounter());
//#endif

//                            if (Len > 0)
//                            {

//                                if ((ComInstance.WriteChannel[USBDEV](pData, Len)) != 0)
//                                {
//                                    //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                                    printf("Daisy OK tx%d\n\r", GetDaisyPushCounter());
//#endif

//                                    DecrementDaisyPushCounter();
//                                    cDaisyPushUpStream(); // Flood upward
//                                    cDaisyPrepareNext();  // Ready for the next sensor in the array
//                                }
//                                else
//                                {
//                                    //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                                    printf("Daisy FAIL in txing %d\n\r", GetDaisyPushCounter());  // TX upstream called
//#endif
//                                }
//                            }

//                        }
//                    }
//                }
//            }
//            else
//            {
//                if (pTxBuf->Writing)
//                {
//                    //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                    printf("007\n\r");
//#endif

//                    if (NULL != ComInstance.WriteChannel[ChNo])
//                    {
//                        //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                        printf("Tx Writing true in the bottom ChNo = %d - PushCounter = %d\n\r", ChNo, GetDaisyPushCounter());
//#endif

//                        if (ComInstance.WriteChannel[ChNo](pTxBuf->Buf, pTxBuf->BlockLen))
//                        {
//                            //#define DEBUG
//#undef DEBUG
//# ifdef DEBUG
//                            printf("008\n\r");
//#endif

//                            pTxBuf->Writing = 0;
//                        }
//                    }

//                }
//            }

//            if (0 == pTxBuf->Writing)
//            {
//                // Tx buffer needs to be empty to fill new data into it....
//                switch (pTxBuf->State)
//                {
//                    case TXFILEUPLOAD:
//                        {
//                            ULONG MsgLeft;

//                            MsgLeft = pTxBuf->MsgLen - pTxBuf->SendBytes;

//                            if (MsgLeft > pTxBuf->BufSize)
//                            {
//                                ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)pTxBuf->BufSize);
//                                pTxBuf->pFile->Pointer += ReadBytes;
//                                pTxBuf->SendBytes += ReadBytes;
//                                pTxBuf->State = TXFILEUPLOAD;
//                            }
//                            else
//                            {
//                                ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)MsgLeft);
//                                pTxBuf->pFile->Pointer += ReadBytes;
//                                pTxBuf->SendBytes += ReadBytes;

//                                if (pTxBuf->MsgLen == pTxBuf->SendBytes)
//                                {
//                                    pTxBuf->State = TXIDLE;

//                                    if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
//                                    { //All Bytes has been read in the file - close handles (it is not GetFile command)

//# ifdef DEBUG
//                                        printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (unsigned long)pTxBuf->pFile->Length);
//#endif

//                                        cComCloseFileHandle(&(pTxBuf->pFile->File));
//                                        cComFreeHandle(pTxBuf->FileHandle);
//                                    }
//                                }
//                            }
//                            if (ReadBytes)
//                            {
//                                pTxBuf->Writing = 1;
//                            }
//                        }
//                        break;

//                    case TXGETFILE:
//                        {
//                            ULONG MsgLeft;

//                            MsgLeft = pTxBuf->MsgLen - pTxBuf->SendBytes;

//                            if (MsgLeft > pTxBuf->BufSize)
//                            {
//                                ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)pTxBuf->BufSize);
//                                pTxBuf->pFile->Pointer += ReadBytes;
//                                pTxBuf->SendBytes += ReadBytes;
//                                pTxBuf->State = TXGETFILE;
//                            }
//                            else
//                            {
//                                ReadBytes = read(pTxBuf->pFile->File, pTxBuf->Buf, (size_t)MsgLeft);
//                                pTxBuf->pFile->Pointer += ReadBytes;
//                                pTxBuf->SendBytes += ReadBytes;

//                                if (pTxBuf->MsgLen == pTxBuf->SendBytes)
//                                {
//                                    pTxBuf->State = TXIDLE;

//                                    if (pTxBuf->pFile->Pointer >= pTxBuf->pFile->Size)
//                                    {

//# ifdef DEBUG
//                                        printf("%s %lu bytes UpLoaded\r\n", pTxBuf->pFile->Name, (unsigned long)pTxBuf->pFile->Length);
//#endif
//                                    }
//                                }
//                            }
//                            if (ReadBytes)
//                            {
//                                pTxBuf->Writing = 1;
//                            }
//                        }
//                        break;

//                    case TXLISTFILES:
//                        {
//                            ULONG TmpN;
//                            ULONG Len;
//                            ULONG NameLen;
//                            ULONG RemCharCnt;
//                            ULONG BytesToSend;
//                            UBYTE Repeat;
//                            char TmpFileName[FILENAMESIZE];

//                            TmpN = pTxBuf->pFile->File;
//                            Len = 0;
//                            Repeat = 1;

//                            if ((pTxBuf->MsgLen - pTxBuf->SendBytes) <= pTxBuf->BufSize)
//                            {
//                                //All requested bytes can be inside the buffer
//                                BytesToSend = (pTxBuf->MsgLen - pTxBuf->SendBytes);
//                                pTxBuf->State = TXIDLE;
//                            }
//                            else
//                            {
//                                BytesToSend = pTxBuf->BufSize;
//                                pTxBuf->State = TXLISTFILES;
//                            }
//                            pTxBuf->BlockLen = BytesToSend;

//                            if (pTxBuf->pFile->Length)
//                            {
//                                // Only here if filename has been divided in 2 pieces
//                                // First transfer the remaining part of the last filename
//                                cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (char*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);
//                                RemCharCnt = NameLen - pTxBuf->pFile->Length;

//                                if (RemCharCnt <= BytesToSend)
//                                {
//                                    // this will fit into the message length
//                                    memcpy((char*)(&(pTxBuf->Buf[Len])), &(TmpFileName[pTxBuf->pFile->Length]), RemCharCnt);
//                                    Len += RemCharCnt;

//                                    free(pTxBuf->pFile->namelist[TmpN]);

//                                    if (RemCharCnt == BytesToSend)
//                                    {
//                                        //if If all bytes is already occupied not more to go right now
//                                        Repeat = 0;
//                                        pTxBuf->pFile->Length = 0;
//                                    }
//                                    else
//                                    {
//                                        Repeat = 1;
//                                    }
//                                }
//                                else
//                                {
//                                    // This is the rare condition if remaining msg len and buf size are almost equal
//                                    memcpy((char*)(&(pTxBuf->Buf[Len])), &(TmpFileName[pTxBuf->pFile->Length]), BytesToSend);
//                                    Len += BytesToSend;
//                                    pTxBuf->pFile->File = TmpN;                         // Adjust Iteration number
//                                    pTxBuf->pFile->Length += Len;
//                                    Repeat = 0;
//                                }
//                            }

//                            while (Repeat)
//                            {

//                                TmpN--;

//                                cComGetNameFromScandirList((pTxBuf->pFile->namelist[TmpN]), (char*)TmpFileName, &NameLen, (UBYTE*)pTxBuf->Folder);

//                                if ((NameLen + Len) <= BytesToSend)                         // Does the next name fit into the buffer?
//                                {
//                                    memcpy((char*)(&(pTxBuf->Buf[Len])), TmpFileName, NameLen);
//                                    Len += NameLen;

//# ifdef DEBUG
//                                    printf("List entry no = %d; File name = %s \r\n", TmpN, pTxBuf->pFile->namelist[TmpN]->d_name);
//#endif

//                                    free(pTxBuf->pFile->namelist[TmpN]);

//                                    if (BytesToSend == Len)
//                                    { // buffer is filled up now exit

//                                        pTxBuf->pFile->Length = 0;
//                                        pTxBuf->pFile->File = TmpN;
//                                        Repeat = 0;
//                                    }
//                                }
//                                else
//                                {
//                                    // No, now fill up to complete buffer size
//                                    ULONG RemCnt;

//                                    RemCnt = BytesToSend - Len;
//                                    memcpy((char*)(&(pTxBuf->Buf[Len])), TmpFileName, RemCnt);
//                                    Len += RemCnt;
//                                    pTxBuf->pFile->Length = RemCnt;
//                                    pTxBuf->pFile->File = TmpN;
//                                    Repeat = 0;
//                                }
//                            }

//                            // Update pointers
//                            pTxBuf->pFile->Pointer += Len;
//                            pTxBuf->SendBytes += Len;

//                            if (pTxBuf->pFile->Pointer == pTxBuf->pFile->Size)
//                            {
//                                // Complete list has been tx'ed
//                                free(pTxBuf->pFile->namelist);
//                                cComFreeHandle(pTxBuf->FileHandle);
//                            }

//                            pTxBuf->Writing = 1;
//                        }
//                        break;

//                    default:
//                        {
//                            // this is idle state
//                        }
//                        break;
//                }
//            }
        }

        public void cComCloseMailBox()
        {
            throw new NotImplementedException();
        }



        public void cComGet()
        {
            throw new NotImplementedException();
        }

        public void cComGetBrickName(sbyte Length, sbyte[] pBrickName)
        {
            throw new NotImplementedException();
        }

        public byte cComGetBtStatus()
        {
            throw new NotImplementedException();
        }

        public RESULT cComGetDeviceData(sbyte Layer, sbyte Port, sbyte Length, ref sbyte pType, ref sbyte pMode, sbyte[] pData)
        {
            throw new NotImplementedException();
        }

        public RESULT cComGetDeviceInfo(sbyte Length, byte[] pInfo)
        {
            throw new NotImplementedException();
        }

        public sbyte cComGetEvent()
        {
            throw new NotImplementedException();
        }

        public sbyte cComGetUsbStatus()
        {
            throw new NotImplementedException();
        }

        public byte cComGetWifiStatus()
        {
            throw new NotImplementedException();
        }





        public void cComOpenMailBox()
        {
            throw new NotImplementedException();
        }

        public void cComRead()
        {
            throw new NotImplementedException();
        }

        public void cComReadData()
        {
            throw new NotImplementedException();
        }

        public void cComReadMailBox()
        {
            throw new NotImplementedException();
        }

        public void cComReady()
        {
            throw new NotImplementedException();
        }

        public void cComReadyMailBox()
        {
            throw new NotImplementedException();
        }

        public void cComRemove()
        {
            throw new NotImplementedException();
        }

        public void cComSet()
        {
            throw new NotImplementedException();
        }

        public RESULT cComSetDeviceInfo(sbyte Length, byte[] pInfo)
        {
            throw new NotImplementedException();
        }

        public RESULT cComSetDeviceType(sbyte Layer, sbyte Port, sbyte Type, sbyte Mode)
        {
            throw new NotImplementedException();
        }

        public void cComTest()
        {
            throw new NotImplementedException();
        }

        public void cComTestMailBox()
        {
            throw new NotImplementedException();
        }





        public void cComWrite()
        {
            throw new NotImplementedException();
        }

        public void cComWriteData()
        {
            throw new NotImplementedException();
        }

        public void cComWriteFile()
        {
            throw new NotImplementedException();
        }

        public void cComWriteMailBox()
        {
            throw new NotImplementedException();
        }
    }
}
