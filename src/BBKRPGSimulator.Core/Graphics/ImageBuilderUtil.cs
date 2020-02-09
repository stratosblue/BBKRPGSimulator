namespace BBKRPGSimulator.Graphics
{
    public static class ImageBuilderUtil
    {
        /// <summary>
        /// 整型数组转换为图片byte数组
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] IntegerArrayToImageBytes(int[] data, int width, int height)
        {
            byte[] result;

            var length = width * height;
            if (data.Length == length)
            {
                result = PixelsToBuffer(data);
            }
            else
            {
                result = new byte[width * height * 4];
                data.CopyTo(result, 0);
            }
            return result;
        }

        /// <summary>
        /// 获取ARGB
        /// </summary>
        /// <param name="color"></param>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public static void ColorToArgb(int color, out byte a, out byte r, out byte g, out byte b)
        {
            var bytes = PixelToByte(color);
            b = bytes[0];
            g = bytes[1];
            r = bytes[2];
            a = bytes[3];
        }

        /// <summary>
        /// 像素集合转换为ARGB图像buffer
        /// 注意：顺序为BGRA
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static byte[] PixelsToBuffer(int[] pixels)
        {
            byte[] result = new byte[pixels.Length * 4];
            for (int i = 0, index = 0; i < pixels.Length && index < result.Length; i++, index += 4)
            {
                result[index] = (byte)(pixels[i] & 0x000000FF); //B
                result[index + 1] = (byte)((pixels[i] & 0x0000FF00) >> 8);  //G
                result[index + 2] = (byte)((pixels[i] & 0x00FF0000) >> 16); //R
                result[index + 3] = (byte)(pixels[i] >> 24);    //A
            }
            return result;
        }

        /// <summary>
        /// 像素转换为ARGB数组
        /// 注意：顺序为BGRA
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static byte[] PixelToByte(int pixel)
        {
            byte[] result = new byte[4];

            result[0] = (byte)(pixel & 0x000000FF); //B
            result[1] = (byte)((pixel & 0x0000FF00) >> 8);  //G
            result[2] = (byte)((pixel & 0x00FF0000) >> 16); //R
            result[3] = (byte)(pixel >> 24);    //A

            return result;
        }

        /// <summary>
        /// 判断区域值
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int RangeValue(int inValue, int minValue, int maxValue)
        {
            if (inValue > maxValue)
            {
                return maxValue;
            }
            if (inValue < minValue)
            {
                return minValue;
            }
            return inValue;
        }
    }
}