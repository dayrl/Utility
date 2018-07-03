using System.Windows.Forms;

namespace Zdd.Utility
{
	/// <summary>
	/// 列表控件助手类。
	/// </summary>
    public static class ListControlHelper
	{
		/// <summary>
		/// 删除列表框选中的项，并保持被删除项相邻项的选中状态。
		/// </summary>
		/// <param name="listBox">将要执行删除操作的列表框。</param>
		public static void RemoveSelectedItems(ListBox listBox)
		{
			int selectedIndex = listBox.SelectedIndex;
			if (selectedIndex == -1)
				return;

			object[] selectedItmes = new object[listBox.SelectedItems.Count];
			listBox.SelectedItems.CopyTo(selectedItmes, 0);

            foreach (object item in selectedItmes)
            {
                listBox.Items.Remove(item);
            }

		    if (selectedIndex < listBox.Items.Count)
				listBox.SelectedIndex = selectedIndex;
			else if (listBox.Items.Count > 0)
				listBox.SelectedIndex = listBox.Items.Count - 1;
			else
				listBox.SelectedIndex = -1;
		}

		/// <summary>
		/// 删除列表框选中的项，并保持被删除项相邻项的选中状态。
		/// </summary>
		/// <param name="listView">将要执行删除操作的列表框。</param>
		public static void RemoveSelectedItems(ListView listView)
		{
			if(listView.SelectedIndices.Count == 0)
				return;

			int selectedIndex = listView.SelectedIndices[0];

			ListViewItem[] selectedItmes = new ListViewItem[listView.SelectedItems.Count];
			listView.SelectedItems.CopyTo(selectedItmes, 0);

            foreach (ListViewItem item in selectedItmes)
            {
                listView.Items.Remove(item);
            }

		    if (selectedIndex < listView.Items.Count)
				listView.Items[selectedIndex].Selected = true;
			else if (listView.Items.Count > 0)
				listView.Items[listView.Items.Count - 1].Selected = true;
		}
	}
}
