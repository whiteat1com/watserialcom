﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;


namespace WATSerialCom
{
  public interface ITXData
  {
    bool IsHexa{get;}
 //   string sText { get; }
    List<byte> byteData { get; }
  }

  public delegate void eventSendButton(ITXData txdata);

  public partial class CtrlTxData : UserControl, ITXData
  {
    #region interface ITXData
    public bool IsHexa
    {
      get { return this.chbHexa.Checked; }
    }
 //   public string sText { get { return this.txbSendData.Text; } }//  System.Text.Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(this.txbSendData.Text)); } }
    public List<byte> byteData
    {
      get
      {
        List<byte> m_byteSendData = new List<byte>();
        if (IsHexa)
        {
          foreach (string s in this.txbSendData.Text.Split(' '))
          {
            if (null != s && "" != s)
              m_byteSendData.Add(Convert.ToByte(s, 16));
          }
          return m_byteSendData;
        }
        else
        {
          m_byteSendData.AddRange(System.Text.Encoding.UTF8.GetBytes(this.txbSendData.Text));
        }
        return m_byteSendData;
      }
    }

    #endregion


    public event eventSendButton SendClicked;
    
    public CtrlTxData()
    {
      InitializeComponent();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      SendText(txbSendData.Text);  
    }

    public void SendText(string str)
    {
      if (null != SendClicked)
      {
        SendClicked(this);
      }
    }

    private void txtSendData_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == Convert.ToInt32(Keys.Enter))
      {
        this.SendText(this.txbSendData.Text);
      }
    } 
  }
}
