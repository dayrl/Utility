using System.Windows.Forms;

namespace Zdd.Utility
{
	/// <summary>
	/// �б�ؼ������ࡣ
	/// </summary>
    public static class ListControlHelper
	{
		/// <summary>
		/// ɾ���б��ѡ�е�������ֱ�ɾ�����������ѡ��״̬��
		/// </summary>
		/// <param name="listBox">��Ҫִ��ɾ���������б��</param>
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
		/// ɾ���б��ѡ�е�������ֱ�ɾ�����������ѡ��״̬��
		/// </summary>
		/// <param name="listView">��Ҫִ��ɾ���������б��</param>
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
