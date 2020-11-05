using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSI2
{
    class ImageProcessor
    {



        public Bitmap ToGrayScale(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    int colorGray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    result.SetPixel(x, y, Color.FromArgb(colorGray, colorGray, colorGray));
                }
            }
            return result;
        }


        public Bitmap ToBinary(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    result.SetPixel(x, y, bitmap.GetPixel(x, y).R >= 140 ? Color.Black : Color.White);
                }
            }


            return result;
        }

        private Color[,] TakeMatrixOfColor(Bitmap bitmap)
        {
            Color[,] colors = new Color[bitmap.Height, bitmap.Width];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    colors[y, x] = bitmap.GetPixel(x, y);
                }
            }
            return colors;
        }


        public string ToStringLabel(int[,] matrix, int width, int height)
        {
            string str = String.Empty;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    str += String.Format("{0,2:00}", matrix[y, x]);
                }
                str += "\n";
            }
            return str;
        }

        public (int[,], List<int>) FirePicture(Bitmap bitmap)
        {
            var color = TakeMatrixOfColor(bitmap);
            var matrix = new int[bitmap.Height, bitmap.Width];
            var matrixResult = new int[bitmap.Height, bitmap.Width];
            matrixResult.Initialize();
            Color backgroundColor = Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    matrix[y, x] = color[y, x] == backgroundColor ? 0 : -1;
                }
            }

            List<int> markers = new List<int>();
            var height = bitmap.Height;
            var width = bitmap.Width;
            FirstStep(matrix, matrixResult, markers, height, width);
            var remove = new List<(int, int)>();
            do
            {
                remove.Clear();
                NormalizeImage(matrixResult, markers, height, width, remove);

            } while (remove.Any());
            return (matrixResult, markers);
        }

        private void FirstStep(int[,] matrix, int[,] matrixResult, List<int> markers, int height, int width)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (matrix[y, x] != 0)
                    {
                        if (markers.Count == 0)
                        {
                            matrixResult[y, x] = 1;
                            markers.Add(1);
                        }
                        else
                        {
                            var neighborsUp = new List<int>();
                            neighborsUp.Add(matrixResult[y, (x - 1) < 0 ? x : x - 1]);
                            neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, (x - 1) < 0 ? x : x - 1]);
                            neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, x]);
                            neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, (x + 1) > (width - 1) ? x : x + 1]);
                            if (neighborsUp.Where(e => e != 0).Any())
                            {
                                matrixResult[y, x] = neighborsUp.Where(e => e != 0).First();
                            }
                            else
                            {
                                markers.Add(markers.Last() + 1);
                                matrixResult[y, x] = markers.Last();
                            }
                        }
                    }
                }
            }
        }

        private void NormalizeImage(int[,] matrixResult, List<int> markers, int height, int width, List<(int, int)> remove)
        {
            foreach (int elem in markers)
            {
                if (!remove.Where(e => e.Item1 == elem).Any())
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // var neighbors = (color[(y - 1) < 0 ? 0 : y-1, x], color[(y + 1) >= height ? y : y+1, x], color[y, (x - 1) < 0 ? x : x-1], color[y, (x + 1) >= width ? x : x+1]);
                            if (matrixResult[y, x] == elem)
                            {
                                var neighborsUp = new List<int>();
                                neighborsUp.Add(matrixResult[y, (x - 1) < 0 ? x : x - 1]);
                                neighborsUp.Add(matrixResult[(y + 1) > (height - 1) ? y : y + 1, x]);
                                neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, x]);
                                neighborsUp.Add(matrixResult[y, (x + 1) > (width - 1) ? x : x + 1]);
                                var temp = neighborsUp.Where(e => e != elem && e != 0);
                                foreach (var el in temp)
                                {
                                    if (!remove.Where(e => e.Item1 == el).Any())
                                    {
                                        remove.Add((el, elem));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // var neighbors = (color[(y - 1) < 0 ? 0 : y-1, x], color[(y + 1) >= height ? y : y+1, x], color[y, (x - 1) < 0 ? x : x-1], color[y, (x + 1) >= width ? x : x+1]);
                    if (remove.Where(e => e.Item1 == matrixResult[y, x]).Any())
                    {
                        matrixResult[y, x] = remove.Where(e => e.Item1 == matrixResult[y, x]).First().Item2;
                    }
                }
            }
            foreach (var el in remove)
            {
                markers.Remove(el.Item1);
            }
        }

        public void Clasterize(Dictionary<int, (int, int, float)> vectors, int countOfClaster, out Dictionary<int, List<int>> clastersVectors)
        {
            Dictionary<int, (int, int, float)> clasters = new Dictionary<int, (int, int, float)>();
            Dictionary<int, (int, int, float)> clastersTemp = new Dictionary<int, (int, int, float)>();
            clastersVectors = new Dictionary<int, List<int>>();
            Random random = new Random();
            for (int i = 0; i < countOfClaster; i++)
            {
                clasters.Add(i, (i, i, i));
                clastersTemp.Add(i, (random.Next(0, 500), random.Next(0, 500), random.Next(0, 500)));
                clastersVectors.Add(i, new List<int>());

            }
            do
            {
                clastersVectors.Clear();
                foreach (var elem in vectors)
                {
                    float distance = -1;
                    int key = 0;
                    foreach (var elemClasters in clasters)
                    {
                        float tempDistance = (float)Math.Sqrt((Math.Pow((elemClasters.Value.Item1 - elem.Value.Item1), 2)
                            + Math.Pow((elemClasters.Value.Item2 - elem.Value.Item2), 2)
                            + Math.Pow((elemClasters.Value.Item3 - elem.Value.Item3), 2)));
                        if (distance == -1 || distance > tempDistance)
                        {
                            key = elemClasters.Key;
                            distance = tempDistance;
                        }
                    }
                    if (!clastersVectors.ContainsKey(key))
                    {
                        clastersVectors.Add(key, new List<int>());
                    }
                    clastersVectors[key].Add(elem.Key);
                }
                foreach (var elem in clastersVectors)
                {
                    int tempDis1 = 0;
                    int tempDis2 = 0;
                    float tempDis3 = 0;
                    foreach (var elVectors in elem.Value)
                    {
                        tempDis1 += Math.Abs(vectors[elVectors].Item1 - clasters[elem.Key].Item1);
                        tempDis2 += Math.Abs(vectors[elVectors].Item2 - clasters[elem.Key].Item2);
                        tempDis3 += (float)Math.Abs(vectors[elVectors].Item3 - clasters[elem.Key].Item3);
                    }
                    tempDis1 /= elem.Value.Count;
                    tempDis2 /= elem.Value.Count;
                    tempDis3 /= elem.Value.Count;
                    clasters.Remove(elem.Key);
                    clasters.Add(elem.Key, (tempDis1, tempDis2, tempDis3));
                }
                if (clasters == clastersTemp)
                    break;
                else
                    clastersTemp = clasters;
            } while (true);
        }


        public Bitmap ToPrintClasterize(int[,] matrix, Bitmap picture, Dictionary<int, List<int>> clastersVectors)
        {
            List<Color> colors = new List<Color>();
            Random random = new Random();
            foreach (var elem in clastersVectors)
            {
                colors.Add(Color.FromArgb(random.Next(1, 244), random.Next(1, 244), random.Next(1, 244)));
            }
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            foreach (var elem in clastersVectors)
            {
                foreach (var elList in elem.Value)
                {
                    pairs.Add(elList, elem.Key);
                }
            }

            for (int y = 0; y < picture.Height; y++)
            {
                for (int x = 0; x < picture.Width; x++)
                {
                    if (pairs.ContainsKey(matrix[y, x]))
                    {
                        picture.SetPixel(x, y, colors[pairs[matrix[y, x]]]);
                    }
                }
            }
            return picture;
        }


        public Dictionary<int, (int, int, float)> ToDictionary(List<int> perimetrs, List<int> squares, List<float> compacts, List<int> markers)
        {
            Dictionary<int, (int, int, float)> result = new Dictionary<int, (int, int, float)>();

            for (int i = 0; i < markers.Count; i++)
            {
                result.Add(markers[i], (perimetrs[i], squares[i], compacts[i]));
            }

            return result;
        }

        public Bitmap ToArea(int[,] matrix, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(matrix[y, x] > 255 ? 255 : matrix[y, x], matrix[y, x] > 255 ? 255 : matrix[y, x], matrix[y, x] > 255 ? 255 : matrix[y, x]));
                }
            }
            return bitmap;
        }

        public Bitmap Resize(Bitmap original, int customWidth, int customHeight)
        {
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            float ratioX = (float)customWidth / (float)originalWidth;
            float ratioY = (float)customHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            var newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }


        public List<int> Square(int[,] matrix, List<int> markers, int width, int height)
        {
            Dictionary<int, int> squares = new Dictionary<int, int>();
            foreach (var elem in markers)
            {
                squares.Add(elem, 0);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if (squares.ContainsKey(matrix[y, x]))
                    {
                        squares[matrix[y, x]] += 1;
                    }
                }
            }
            return squares.Values.ToList();
        }
        public List<int> Perimetr(int[,] matrix, List<int> markers, int width, int height)
        {
            Dictionary<int, int> perimetr = new Dictionary<int, int>();
            foreach (var elem in markers)
            {
                perimetr.Add(elem, 0);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var neighborsUp = new List<int>();
                    neighborsUp.Add(matrix[y, (x - 1) < 0 ? x : x - 1]);
                    neighborsUp.Add(matrix[(y + 1) > (height - 1) ? y : y + 1, x]);
                    neighborsUp.Add(matrix[(y - 1) < 0 ? 0 : y - 1, x]);
                    neighborsUp.Add(matrix[y, (x + 1) > (width - 1) ? x : x + 1]);
                    var temp = neighborsUp.Where(e => e == 0);
                    if (perimetr.ContainsKey(matrix[y, x]) && (temp.Any() || y == 0 || x == 0 || y == height - 1 || x == width - 1))
                    {
                        perimetr[matrix[y, x]] += 1;
                    }
                }
            }
            return perimetr.Values.ToList();
        }

        public List<float> Compact(List<int> perimetrs, List<int> squares)
        {
            List<float> compacts = new List<float>();
            for (int i = 0; i < perimetrs.Count; i++)
            {
                compacts.Add((float)Math.Pow(perimetrs[i], 2) / (float)squares[i]);
            }
            return compacts;
        }
    }
}


/*var neighborsMatrix = (matrix[(y - 1) < 0 ? 0 : y - 1, x], matrix[(y + 1) >= height ? y : y + 1, x], matrix[y, (x - 1) < 0 ? x : x - 1], matrix[y, (x + 1) >= width ? x : x + 1]);
if (neighborsMatrix.Item1 == (0)
    && neighborsMatrix.Item2 == (0)
    && neighborsMatrix.Item3 == (0)
    && neighborsMatrix.Item4 == (0))
{
    matrixResult[y, x] = 0;
}
else
{
    if (markers.Count == 0)
    {
        markers.Add(1);
        matrixResult[y, x] = 1;
    }
    else if ((neighborsMatrix.Item1 == (0) || neighborsMatrix.Item1 == -1)
          && (neighborsMatrix.Item2 == (0) || neighborsMatrix.Item2 == -1)
          && (neighborsMatrix.Item3 == (0) || neighborsMatrix.Item3 == -1)
          && (neighborsMatrix.Item4 == (0) || neighborsMatrix.Item4 == -1))
    {
        var neighborsUp = new List<int>();
        neighborsUp.Add(matrixResult[y, (x - 1) < 0 ? x : x - 1]);
        neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, (x - 1) < 0 ? x : x - 1]);
        neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, x]);
        neighborsUp.Add(matrixResult[(y - 1) < 0 ? 0 : y - 1, (x + 1) > (width - 1) ? x : x + 1]);

        if (neighborsUp.Any(e => e != -1 && e != 0))
        {
            matrixResult[y, x] = neighborsUp.Where(e => e != -1 && e != 0).First();
        }
        else
        {
            markers.Add(markers.Last() + 1);
            matrixResult[y, x] = markers.Last();
        }
        markers.Add(markers.Last() + 1);
        matrix[y, x] = markers.Last();
    }
    else
    {
        matrixResult[y, x] = (neighborsMatrix.Item1 != (0) && neighborsMatrix.Item1 != -1) ? neighborsMatrix.Item1
            : (neighborsMatrix.Item3 != (0) && neighborsMatrix.Item3 != -1) ? neighborsMatrix.Item3
            : (neighborsMatrix.Item4 != (0) && neighborsMatrix.Item4 != -1) ? neighborsMatrix.Item4
            : (neighborsMatrix.Item2 != (0) && neighborsMatrix.Item2 != -1) ? neighborsMatrix.Item2 : 0;
    }
}*/