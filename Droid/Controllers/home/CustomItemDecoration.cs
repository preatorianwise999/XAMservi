using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;

namespace ServipagMobile.Droid {
	public class CustomItemDecoration : RecyclerView.ItemDecoration {
		private Drawable divider;

		public CustomItemDecoration(Drawable divider) {
			this.divider = divider;
		}

		public override void GetItemOffsets(Android.Graphics.Rect outRect, View view, RecyclerView parent, RecyclerView.State state) {
			base.GetItemOffsets(outRect, view, parent, state);

			if (parent.GetChildAdapterPosition(view) == 0) {
				return;
			}

			outRect.Top = divider.IntrinsicHeight;
		}

		public override void OnDraw(Android.Graphics.Canvas cValue, RecyclerView parent, RecyclerView.State state) {
			base.OnDraw(cValue, parent, state);

			int left = parent.PaddingLeft;
			int right = parent.Width - parent.PaddingRight;

			for (int i = 0; i < parent.ChildCount; i++) {
				View child = parent.GetChildAt(i);

				var parameters = child.LayoutParameters.JavaCast<RecyclerView.LayoutParams>();

				int top = child.Bottom + parameters.BottomMargin;
				int bottom = top + divider.IntrinsicHeight;

				divider.SetBounds(left, top, right, bottom);
				divider.Draw(cValue);
			}
		}
	}
}
