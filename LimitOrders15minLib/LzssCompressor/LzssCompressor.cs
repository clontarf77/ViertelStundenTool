//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ViertelStdToolLib.LzssCompressor
//{
//    public class LzssCompressor : ILzssCompressor
//    {
//        char[] inBuffer;
//        char[] outBuffer;
//        int inBufferLen;
//        int inBufferCurPos;
//        int outBufferCurPos;
//        public static readonly int LZSS_BUFFER_SIZE = 1024000;
//        private static readonly int EOF = -1;
//        private static readonly int N = 4096;
//        private static readonly int F = 18;
//        private static readonly int THRESHOLD = 2;
//        private static readonly int NIL = 4096;

//        private long textsize = 0L;
//        private long codesize = 0L;
//        private long printcount = 0L;
//        private char[] text_buf = new char['ထ'];
//        private int match_position;
//        private int match_length;
//        private int[] lson = new int['ခ'];
//        private int[] rson = new int['ᄁ'];
//        private int[] dad = new int['ခ'];

//        #region Compress
//        public int Compress(char[] inBuf, int inBufSize, char[] outBuf, int outBufSize)
//        {
//            if (inBufSize <= 0)
//            {
//                //throw new RuntimeException("Error at Compress: empty in-buffer");
//            }
//            char[] str = new char[outBufSize];

//            int size = LZSS_Encode(inBuf, inBufSize, str);
//            if (size > outBufSize)
//            {
//                //throw new RuntimeException("Error at Compress: out-buffer overflow");
//            }
//            size = AsciiMasking(str, size, outBuf);
//            return size;
//        }
//        #endregion

//        #region Decompress
//        public int Decompress(char[] inBuf, int inBufSize, char[] outBuf, int outBufSize)
//        {
//            int strsize = outBufSize > 1024 ? outBufSize : 1024;
//            char[] str = new char[strsize];
//            int size = AsciiUnMasking(inBuf, str);
//            if (size > strsize)
//            {
//                //throw new RuntimeException("Error at decompress: unmask overflow");
//            }
//            size = LZSS_Decode(str, size, outBuf);
//            if (size > outBufSize)
//            {
//                //throw new RuntimeException("Error at decompress: result too big");
//            }
//            return size;
//        }
//        #endregion

//        #region Verify compression
//        public Boolean VerifyCompression(char[] originalData, String compressedData)
//        {
//            char[] decompressBuffer = new char[1024000];
//            char[] compressedDataArray = compressedData.ToCharArray();
//            int size = Decompress(compressedDataArray, compressedDataArray.Length, decompressBuffer, decompressBuffer.Length);
//            if (size != originalData.Length)
//            {
//                return false;
//            }
//            for (int i = 0; i < size; i++)
//            {
//                if (decompressBuffer[i] != originalData[i])
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//        #endregion

//        private int GetC()
//        {
//            if (this.inBufferCurPos == this.inBufferLen)
//            {
//                return -1;
//            }
//            int c = 255;
//            return this.inBuffer[(this.inBufferCurPos++)] & c;
//        }

//        private void PutC(char c)
//        {
//            this.outBuffer[(this.outBufferCurPos++)] = ((char)(c & 0xFF));
//        }

//        private void InitTree()
//        {
//            for (int i = 4097; i <= 4352; i++)
//            {
//                this.rson[i] = 4096;
//            }
//            for (int i = 0; i < 4096; i++)
//            {
//                this.dad[i] = 4096;
//            }
//        }

//        private void InsertNode(int r)
//        {
//            int cmp = 1;

//            int p = 'ခ' + this.text_buf[r];

//            this.rson[r] = 4096;
//            this.lson[r] = 4096;
//            this.match_length = 0;
//            for (; ; )
//            {
//                if (cmp >= 0)
//                {
//                    if (this.rson[p] != 4096)
//                    {
//                        p = this.rson[p];
//                    }
//                    else
//                    {
//                        this.rson[p] = r; this.dad[r] = p;
//                    }
//                }
//                else if (this.lson[p] != 4096)
//                {
//                    p = this.lson[p];
//                }
//                else
//                {
//                    this.lson[p] = r; this.dad[r] = p; return;
//                }
//                int i;
//                for (i = 1; i < 18; i++)
//                {
//                    if ((cmp = this.text_buf[(r + i)] - this.text_buf[(p + i)]) != 0)
//                    {
//                        break;
//                    }
//                }
//                if (i > this.match_length)
//                {
//                    this.match_position = p;
//                    if ((this.match_length = i) >= 18)
//                    {
//                        break;
//                    }
//                }
//            }
//            this.dad[r] = this.dad[p]; this.lson[r] = this.lson[p]; this.rson[r] = this.rson[p];
//            this.dad[this.lson[p]] = r; this.dad[this.rson[p]] = r;
//            if (this.rson[this.dad[p]] == p)
//            {
//                this.rson[this.dad[p]] = r;
//            }
//            else
//            {
//                this.lson[this.dad[p]] = r;
//            }
//            this.dad[p] = 4096;
//        }

//        private void DeleteNode(int p)
//        {
//            if (this.dad[p] == 4096)
//            {
//                return;
//            }
//            int q;
//            if (this.rson[p] == 4096)
//            {
//                q = this.lson[p];
//            }
//            else
//            {
//                if (this.lson[p] == 4096)
//                {
//                    q = this.rson[p];
//                }
//                else
//                {
//                    q = this.lson[p];
//                    if (this.rson[q] != 4096)
//                    {
//                        do
//                        {
//                            q = this.rson[q];
//                        } while (this.rson[q] != 4096);
//                        this.rson[this.dad[q]] = this.lson[q]; this.dad[this.lson[q]] = this.dad[q];
//                        this.lson[q] = this.lson[p]; this.dad[this.lson[p]] = q;
//                    }
//                    this.rson[q] = this.rson[p]; this.dad[this.rson[p]] = q;
//                }
//            }
//            this.dad[q] = this.dad[p];
//            if (this.rson[this.dad[p]] == p)
//            {
//                this.rson[this.dad[p]] = q;
//            }
//            else
//            {
//                this.lson[this.dad[p]] = q;
//            }
//            this.dad[p] = 4096;
//        }

//        private void Encode()
//        {
//            char[] code_buf = new char[17];

//            InitTree();
//            code_buf[0] = '\0';

//            int code_buf_ptr = 1;
//            char mask = (char)'1';
//            int s = 0; int r = 4078;
//            for (int i = s; i < r; i++)
//            {
//                this.text_buf[i] = ' ';
//            }
//            int c;
//            int len;
//            for (len = 0; (len < 18) && ((c = GetC()) != -1); len++)
//            {
//                this.text_buf[(r + len)] = ((char)c);
//            }
//            if ((this.textsize = len) == 0L)
//            {
//                return;
//            }
//            for (int i = 1; i <= 18; i++)
//            {
//                InsertNode(r - i);
//            }
//            InsertNode(r);
//            do
//            {
//                if (this.match_length > len)
//                {
//                    this.match_length = len;
//                }
//                if (this.match_length <= 2)
//                {
//                    this.match_length = 1; int
//                      tmp157_156 = 0; char[] tmp157_154 = code_buf; tmp157_154[tmp157_156] = ((char)(tmp157_154[tmp157_156] | mask));
//                    code_buf[(code_buf_ptr++)] = this.text_buf[r];
//                }
//                else
//                {
//                    code_buf[(code_buf_ptr++)] = ((char)this.match_position);
//                    code_buf[(code_buf_ptr++)] = ((char)(this.match_position >> 4 & 0xF0 | this.match_length - 3));
//                }
//                mask = (char)(mask << (char)'1' & 0xFF);
//                if (mask == 0)
//                {
//                    for (int i_ = 0; i_ < code_buf_ptr; i_++)
//                    {
//                        PutC(code_buf[i_]);
//                    }
//                    this.codesize += code_buf_ptr;
//                    code_buf[0] = '\0';
//                    code_buf_ptr = 1;
//                    mask = (char)'1';
//                }
//                int last_match_length = this.match_length;
//                //int c;
//                int i;
//                for (i = 0; (i < last_match_length) && ((c = GetC()) != -1); i++)
//                {
//                    DeleteNode(s);
//                    this.text_buf[s] = ((char)c);
//                    if (s < 17)
//                    {
//                        this.text_buf[(s + 4096)] = ((char)c);
//                    }
//                    s = s + 1 & 0xFFF; r = r + 1 & 0xFFF;

//                    InsertNode(r);
//                }
//                while (i++ < last_match_length)
//                {
//                    DeleteNode(s);
//                    s = s + 1 & 0xFFF; r = r + 1 & 0xFFF;

//                    len--;
//                    if (len != 0)
//                    {
//                        InsertNode(r);
//                    }
//                }
//            } while (len > 0);
//            if (code_buf_ptr > 1)
//            {
//                for (int i = 0; i < code_buf_ptr; i++)
//                {
//                    PutC(code_buf[i]);
//                }
//                this.codesize += code_buf_ptr;
//            }
//        }

//        private void Decode()
//        {
//            for (int i = 0; i < 4078; i++)
//            {
//                this.text_buf[i] = ' ';
//            }
//            int r = 4078; char flags = '\0';
//            for (; ; )
//            {
//                if (((flags = (char)(flags >> (char)'1')) & 0x100) == 0)
//                {
//                    int c;
//                    if ((c = GetC()) == -1)
//                    {
//                        break;
//                    }
//                    flags = (char)(c | 0xFF00);
//                }
//                if ((flags & 0x1) != 0)
//                {
//                    int c;
//                    if ((c = GetC()) == -1)
//                    {
//                        break;
//                    }
//                    PutC((char)c); this.text_buf[(r++)] = ((char)c); r &= 0xFFF;
//                }
//                else
//                {
//                    int i;
//                    int j;
//                    if (((i = GetC()) == -1) ||
//                      ((j = GetC()) == -1))
//                    {
//                        break;
//                    }
//                    i |= (j & 0xF0) << 4; j = (j & 0xF) + 2;
//                    for (int k = 0; k <= j; k++)
//                    {
//                        int c = this.text_buf[(i + k & 0xFFF)];
//                        PutC((char)c); this.text_buf[(r++)] = ((char)c); r &= 0xFFF;
//                    }
//                }
//            }
//        }

//        private void initBuffers(char[] inBuf, int dataLength, char[] outBuf)
//        {
//            this.inBuffer = inBuf;
//            this.inBufferCurPos = 0;
//            this.inBufferLen = dataLength;
//            this.outBuffer = outBuf;
//            this.outBufferCurPos = 0;
//        }

//        private int LZSS_Encode(char[] inBuf, int dataLength, char[] outBuf)
//        {
//            initBuffers(inBuf, dataLength, outBuf);
//            Encode();
//            return this.outBufferCurPos;
//        }

//        private int LZSS_Decode(char[] inBuf, int dataLength, char[] outBuf)
//        {
//            initBuffers(inBuf, dataLength, outBuf);
//            Decode();
//            return this.outBufferCurPos;
//        }

//        private char unsignedSub(char val, int subVal)
//        {
//            int iRetVal = val - subVal;
//            char ret;
//            if (iRetVal < 0)
//            {
//                byte b = (byte)iRetVal;
//                ret = (char)(255 - (b ^ 0xFFFFFFFF));
//            }
//            else
//            {
//                ret = (char)iRetVal;
//            }
//            return ret;
//        }

//        private int AsciiUnMasking(char[] inBuf, char[] outBuf)
//        {
//            int size = 0;
//            int inBuf_ptr = 0;
//            int outBuf_ptr = 0;
//            while ((inBuf_ptr < inBuf.Length) && (inBuf[inBuf_ptr] != 0))
//            {
//                switch (inBuf[inBuf_ptr])
//                {
//                    case '"':
//                        inBuf_ptr++;
//                        if (inBuf[inBuf_ptr] <= '[')
//                        {
//                            outBuf[outBuf_ptr] = unsignedSub(inBuf[inBuf_ptr], 168);
//                        }
//                        else
//                        {
//                            outBuf[outBuf_ptr] = unsignedSub(inBuf[inBuf_ptr], 169);
//                        }
//                        inBuf_ptr++;
//                        break;
//                    case '!':
//                        inBuf_ptr++;
//                        if (inBuf[inBuf_ptr] <= '[')
//                        {
//                            outBuf[outBuf_ptr] = unsignedSub(inBuf[inBuf_ptr], 82);
//                        }
//                        else if (inBuf[inBuf_ptr] <= 'z')
//                        {
//                            outBuf[outBuf_ptr] = unsignedSub(inBuf[inBuf_ptr], 83);
//                        }
//                        else if (inBuf[inBuf_ptr] == '{')
//                        {
//                            outBuf[outBuf_ptr] = '\\';
//                        }
//                        else if (inBuf[inBuf_ptr] == '|')
//                        {
//                            outBuf[outBuf_ptr] = '';
//                        }
//                        else
//                        {
//                            //throw new RuntimeException("Error at AsciiUnMasking: invalid ASCII mask code");
//                        }
//                        inBuf_ptr++;
//                        break;
//                    default:
//                        outBuf[outBuf_ptr] = ((char)(inBuf[(inBuf_ptr++)] & 0xFF));
//                        break;
//                }
//                outBuf_ptr++;
//                size++;
//            }
//            return size;
//        }

//        private int AsciiMasking(char[] inBuf, int inBufSize, char[] outBuf)
//        {
//            int size = inBufSize;

//            int inBuf_ptr = 0;
//            int outBuf_ptr = 0;
//            for (int j = inBufSize; j > 0; j--)
//            {
//                byte ch = (byte)(inBuf[(inBuf_ptr++)] & 0xFF);
//                if (ch < -76)
//                {
//                    outBuf[(outBuf_ptr++)] = '"';
//                    outBuf[(outBuf_ptr++)] = ((char)(ch + 168));
//                    size++;
//                }
//                else if ((ch >= -76) && (ch < -42))
//                {
//                    outBuf[(outBuf_ptr++)] = '"';
//                    outBuf[(outBuf_ptr++)] = ((char)(ch + 169));
//                    size++;
//                }
//                else if ((ch >= -42) && (ch < 10))
//                {
//                    outBuf[(outBuf_ptr++)] = '!';
//                    outBuf[(outBuf_ptr++)] = ((char)(ch + 82));
//                    size++;
//                }
//                else if ((ch >= 10) && (ch < 40))
//                {
//                    outBuf[(outBuf_ptr++)] = '!';
//                    outBuf[(outBuf_ptr++)] = ((char)(ch + 83));
//                    size++;
//                }
//                else if (ch == 92)
//                {
//                    outBuf[(outBuf_ptr++)] = '!';
//                    outBuf[(outBuf_ptr++)] = '{';
//                    size++;
//                }
//                else if (ch == Byte.MaxValue)
//                {
//                    outBuf[(outBuf_ptr++)] = '!';
//                    outBuf[(outBuf_ptr++)] = '|';
//                    size++;
//                }
//                else
//                {
//                    outBuf[(outBuf_ptr++)] = ((char)ch);
//                }
//            }
//            outBuf[outBuf_ptr] = '\0';
//            size++;
//            return size;
//        }


//    }
//}


//// TODO add runtime exceptions again.