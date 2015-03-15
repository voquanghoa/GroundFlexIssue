
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace TestClipPath
{
	public class ClippedCanvas : View
	{
		private Bitmap m_Bitmap;

		public ClippedCanvas (Context context) :
			base (context)
		{
			Initialize ();
		}

		public ClippedCanvas (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public ClippedCanvas (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		private void Initialize ()
		{
			m_Bitmap = BitmapFactory.DecodeResource (Resources, Resource.Drawable.thuthui);


		}

		protected override void OnDraw (Canvas canvas)
		{
			Rect imgRect = new Rect (0, 0, m_Bitmap.Width, m_Bitmap.Height);
			Rect canvasRect = new Rect (0, 0, Width, Height);


			Path clipPath = new Path ();
			clipPath.MoveTo (100, 100);
			clipPath.LineTo (150, 100);
			clipPath.LineTo (200, 200);
			clipPath.LineTo (400, 300);
			clipPath.LineTo (100, 300);
			clipPath.Close ();

			Bitmap drawBm = CreateClippedBitmap (m_Bitmap, clipPath);

			canvas.DrawBitmap (drawBm, imgRect, canvasRect, new Paint ());


			base.OnDraw (canvas);
		}

		private Bitmap CreateClippedBitmap(Bitmap bmOriginal,Path pathMask)
		{
			int width = bmOriginal.Width;
			int height = bmOriginal.Height;
			int size = width * height;

			Bitmap bmMask = Bitmap.CreateBitmap (width, height, Bitmap.Config.Argb8888);

			Paint bmPaint = new Paint ();
			bmPaint.Color = Color.Red;

			Canvas cvMask = new Canvas (bmMask);
			cvMask.DrawPath (pathMask,bmPaint);

			int[] pixelMask = new int[size];
			bmMask.GetPixels (pixelMask, 0,width, 0, 0, width, height);

			for (int i = 0; i < pixelMask.Length; i++) {
				pixelMask [i] <<= 8;
			}

			Bitmap result = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888); 
			Paint paintResult = new Paint ();
			paintResult.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

			Canvas cvResult = new Canvas (result);

			cvResult.DrawBitmap (bmOriginal, 0, 0, null);
			cvResult.DrawBitmap (pixelMask,0, width, 0, 0, width, height, true, paintResult);

			return result;
		}

	}
}

