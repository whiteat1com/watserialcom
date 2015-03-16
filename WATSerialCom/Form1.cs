using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WATSerialCom
{
  public partial class Form1 : Form
  {
    // test 3
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      //////////////////////////////////////////////////////////////////////////
      // 사용가능한 시리얼 포트 얻기
      try
      {
         cbPortName.Items.AddRange(SerialPort.GetPortNames());
      } catch { }

      if (cbPortName.Items.Count > 0)
        cbPortName.SelectedIndex = 0;  // 컴포트 정보가 없을 경우 컴포트의 0번째를 사용
 
    }

    private void OpenComm()
    {
      try
      {
        // sp1 값이 null 일때만 새로운 SerialPort 를 생성합니다.
        if (!sp1.IsOpen)
        {
          sp1.PortName = this.cbPortName.Text;
          sp1.BaudRate = GetSelectedBaudRate();

          sp1.RtsEnable = true;
          sp1.Open();

          RefreshViewControl(true);

        }
 
      } catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
    
    private void CloseComm( )
    {
      // sp1 이 null 아닐때만 close 처리를 해준다.
      if (null != sp1)
      {
        if (sp1.IsOpen)
        {
          sp1.Close();
        }
      }
      RefreshViewControl(false);
    }

    private void RefreshViewControl(bool _opened)
    {
      if (btnOpen.Enabled == _opened)
        btnOpen.Enabled = !_opened;
      if (btnClose.Enabled != _opened)
        btnClose.Enabled = _opened;
      if (ctrlTxData1.Enabled != _opened)
        ctrlTxData1.Enabled = _opened;

      if(splitContainer1.Enabled != _opened) splitContainer1.Enabled = _opened;
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      this.OpenComm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.CloseComm();
    }

    private void cbPortName_MouseDown(object sender, MouseEventArgs e)
    {
      cbPortName.Items.Clear();
      cbPortName.Items.AddRange(SerialPort.GetPortNames());
    }

    private Int32 GetSelectedBaudRate()
    {
      if (this.rdb600.Checked)
        return 600;
      else if (this.rdb1200.Checked)
        return 1200;
      else if (this.rdb2400.Checked)
        return 2400;
      else if (this.rdb4800.Checked)
        return 4800;
      else if (this.rdb9600.Checked)
        return 9600;

      else if (this.rdb14400.Checked)
        return 14400;
      else if (this.rdb19200.Checked)
        return 19200;
      else if (this.rdb28800.Checked)
        return 28800;
      else if (this.rdb38400.Checked)
        return 38400;
      else if (this.rdb56000.Checked)
        return 56000;

      else if (this.rdb57600.Checked)
        return 57600;
      else if (this.rdb115200.Checked)
        return 115200;
      else if (this.rdb128000.Checked)
        return 128000;
      else if (this.rdb256000.Checked)
        return 256000;
      else
      {
        try
        {
          return Convert.ToInt32(this.txbBaudRate.Text);
        } catch
        {
          return 9600;

        }
      }
    }

    private void tsbRxViewText_Click(object sender, EventArgs e)
    {
      this.item_Click(sender, e);
    }

    private void item_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem item = sender as ToolStripMenuItem;

      this.txbRXText.Clear();
      this.txbRXHex.Clear();

      foreach (ToolStripMenuItem tempItemp in ((ToolStripMenuItem)item.OwnerItem).DropDownItems)
      {
        if (tempItemp == item)
        {
          tempItemp.Checked = true;
          if (item == tsbRxViewText)
          {
            splitContainer1.Panel2Collapsed = true;
          }
          else if (item == tsbRxViewHex)
          {
            splitContainer1.Panel1Collapsed = true;
          }
          else if (item == tsbRxViewBoth)
          {
            splitContainer1.Panel1Collapsed = splitContainer1.Panel2Collapsed = false;
          }
        }
        else
          tempItemp.Checked = false;
      }
    }

    private void tsbRxViewHexa_Click(object sender, EventArgs e)
    {
      this.item_Click(sender, e);
    }

    private void tsbRxViewBoth_Click(object sender, EventArgs e)
    {
      this.item_Click(sender, e);
    }

    private void ctrlTxData1_SendClicked(ITXData oSendData)
    {
      Send(oSendData);
    }

    private void Send(ITXData oSendDatas)
    {
      if(sp1 == null) return;
      if(!sp1.IsOpen  ) return;

      try
      {
        sp1.Write(oSendDatas.byteData.ToArray(), 0, oSendDatas.byteData.Count);
      } catch { }
    } 
          
    // 데이터를 받으면
    private void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {  
        int iRecSize = sp1.BytesToRead; // 수신된 데이터 갯수

        if (iRecSize != 0) // 수신된 데이터의 수가 0이 아닐때만 처리하자
        {
//          Console.Write("rx:" + iRecSize.ToString());

          byte[] buff = new byte[iRecSize];
          try
          {
            sp1.Read(buff, 0, iRecSize);
 
            if(this.tsbRxViewText.Checked || this.tsbRxViewBoth.Checked)
              txbRXText.AppendText(System.Text.Encoding.UTF8.GetString(buff, 0, buff.Length));

            if (this.tsbRxViewHex.Checked || this.tsbRxViewBoth.Checked)
              txbRXHex.AppendText(BitConverter.ToString(buff).Replace("-", " ") + " ");
          } catch { }
         }
      } 
      catch (System.Exception)
      {
      } 
    }
  }
}
