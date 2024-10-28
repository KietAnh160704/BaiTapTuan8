using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'schoolidbDataSet.Student' table. You can move, or remove it, as needed.
            this.studentTableAdapter.Fill(this.schoolidbDataSet.Student);
            dgvSinhVien.DataSource = this.schoolidbDataSet.Student;
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fullname = dgvSinhVien.Rows[e.RowIndex].Cells[1].Value;
                var age = dgvSinhVien.Rows[e.RowIndex].Cells[2].Value;
                var major = dgvSinhVien.Rows[e.RowIndex].Cells[3].Value;
                textBox1.Text = fullname?.ToString();
                textBox2.Text = age?.ToString();
                if (major != null)
                {
                    comboBox1.SelectedItem = major.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            var newRow = schoolidbDataSet.Student.NewRow();
            newRow["Fullname"] = textBox1.Text;
            newRow["Age"] = int.Parse(textBox2.Text);
            newRow["Major"] = comboBox1.SelectedItem.ToString();
            schoolidbDataSet.Student.Rows.Add(newRow);
            try
            {
                studentTableAdapter.Update(schoolidbDataSet.Student);
                MessageBox.Show("Thêm sinh viên thành công!");

                this.studentTableAdapter.Fill(this.schoolidbDataSet.Student);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            // Lấy chỉ số hàng hiện tại
            int rowIndex = dgvSinhVien.CurrentRow.Index;

            // Lấy StudentID từ dòng hiện tại (Giả sử StudentID ở cột đầu tiên)
            int studentId = (int)dgvSinhVien.Rows[rowIndex].Cells[0].Value;

            // Cập nhật thông tin sinh viên
            try
            {
                var row = schoolidbDataSet.Student.Rows[rowIndex];
                row["Fullname"] = textBox1.Text;
                row["Age"] = int.Parse(textBox2.Text);
                row["Major"] = comboBox1.SelectedItem.ToString();

                // Gọi Update
                studentTableAdapter.Update(schoolidbDataSet.Student);
                MessageBox.Show("Cập nhật thông tin sinh viên thành công!");

                // Làm mới DataGridView
                this.studentTableAdapter.Fill(this.schoolidbDataSet.Student);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
                return;
            }

            // Lấy thông tin sinh viên hiện tại
            int studentId = (int)dgvSinhVien.CurrentRow.Cells[0].Value;
            string fullname = dgvSinhVien.CurrentRow.Cells[1].Value.ToString();
            int age = (int)dgvSinhVien.CurrentRow.Cells[2].Value;
            string major = dgvSinhVien.CurrentRow.Cells[3].Value.ToString();

            // Xác nhận việc xóa
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                 "Xác nhận xóa",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi phương thức Delete với đầy đủ tham số
                    studentTableAdapter.Delete(studentId, fullname, age, major);

                    MessageBox.Show("Đã xóa sinh viên thành công!");

                    // Làm mới DataGridView
                    this.studentTableAdapter.Fill(this.schoolidbDataSet.Student);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }
    }
    
}
