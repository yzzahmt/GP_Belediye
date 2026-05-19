using System;
using System.IO;
using System.Text.RegularExpressions;
class P {
    static void Main() {
        string t = File.ReadAllText(""Views\\PersonelHomeScreen.cs"");
        t = Regex.Replace(t, @""var bCikis = new Button \{ Text = \""([^\""]*?)Yap\"""", @""var bCikis = new Button { Text = """"  🚪  Çıkış Yap"""""");
        t = Regex.Replace(t, @""var bBildirimGonder = new Button \{ Text = \""([^\""]*?)Bildirim G([^\""]*?)nder\"""", @""var bBildirimGonder = new Button { Text = """"  📢  Bildirim Gönder"""""");
        t = Regex.Replace(t, @""var bDuyuru = new Button \{ Text = \""([^\""]*?)Duyuru Ekle\"""", @""var bDuyuru = new Button { Text = """"  📌  Duyuru Ekle"""""");
        t = Regex.Replace(t, @""var bPersonel = new Button \{ Text = \""([^\""]*?)Personel Y([^\""]*?)netimi\"""", @""var bPersonel = new Button { Text = """"  👥  Personel Yönetimi"""""");
        t = Regex.Replace(t, @""var bVatandas = new Button \{ Text = \""([^\""]*?)Vatanda([^\""]*?) Y([^\""]*?)netimi\"""", @""var bVatandas = new Button { Text = """"  🧑‍💼  Vatandaş Yönetimi"""""");
        t = Regex.Replace(t, @""var bDilekce = new Button \{ Text = \""([^\""]*?)Gelen Dilekce([^\""]*?)\"""", @""var bDilekce = new Button { Text = """"  📨  Gelen Dilekce/Basvurular"""""");
        t = Regex.Replace(t, @""var bBasvuru = new Button \{ Text = \""([^\""]*?)Basvuru Y([^\""]*?)netimi\"""", @""var bBasvuru = new Button { Text = """"  📋  Basvuru Yönetimi"""""");
        t = Regex.Replace(t, @""var bBildirim = new Button \{ Text = \""([^\""]*?)Bildirimler\"""", @""var bBildirim = new Button { Text = """"  🔔  Bildirimler"""""");
        t = Regex.Replace(t, @""var bAnaSayfa = new Button \{ Text = \""([^\""]*?)Ana Sayfa\"""", @""var bAnaSayfa = new Button { Text = """"  🏠  Ana Sayfa"""""");
        
        t = Regex.Replace(t, @""HeaderText = \""G([^\""]*?)nderen\"""", @""HeaderText = """"Gönderen"""""");
        t = Regex.Replace(t, @""HeaderText = \""Ba([^\""]*?)l([^\""]*?)k\"""", @""HeaderText = """"Başlık"""""");
        t = Regex.Replace(t, @""HeaderText = \""([^\""]*?)erik\"""", @""HeaderText = """"İçerik"""""");
        
        File.WriteAllText(""Views\\PersonelHomeScreen.cs"", t, new System.Text.UTF8Encoding(true));
    }
}
