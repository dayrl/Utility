using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Zdd.Utility
{
    /// <summary>
    /// FormHelper
    /// </summary>
    public class FormHelper
    {
        /// <summary>
        /// Creates the form.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="show">if set to <c>true</c> [show].</param>
        /// <returns></returns>
        public static Form CreateForm(string name, int width, int height, bool show)
        {
            Form form = new Form();
            form.Name = name;
            form.Text = "Utility";
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            if (width > 0)
            {
                form.Width = width;
            }
            if (height > 0)
            {
                form.Height = height;
            }
            if (show)
            {
                form.Show();
            }
            return form;
        }

        /// <summary>
        /// Creates the rich text box.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parentForm">The parent form.</param>
        /// <returns></returns>
        public static RichTextBox CreateRichTextBox(string name, Form parentForm)
        {
            RichTextBox box = new RichTextBox();
            box.Dock = DockStyle.Fill;
            box.Location = new Point(0, 0);
            box.Name = name;
            box.Size = new Size(parentForm.Width, parentForm.Height);
            parentForm.Controls.Add(box);
            return box;
        }

        /// <summary>
        /// Finds the control.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="searchControl">The search control.</param>
        /// <returns></returns>
        public static Control FindControl(string name, Control searchControl)
        {
            if (searchControl.Name == name)
            {
                return searchControl;
            }
            foreach (Control control in searchControl.Controls)
            {
                Control control2 = FindControl(name, control);
                if (control2 != null)
                {
                    return control2;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the control.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="searchControl">The search control.</param>
        /// <param name="controlType">Type of the control.</param>
        /// <returns></returns>
        public static Control FindControl(string name, Control searchControl, System.Type controlType)
        {
            if ((searchControl.Name == name) && (searchControl.GetType() == controlType))
            {
                return searchControl;
            }
            foreach (Control control in searchControl.Controls)
            {
                Control control2 = FindControl(name, control, controlType);
                if (control2 != null)
                {
                    return control2;
                }
            }
            return null;
        }
    }
}
