namespace ShoeStoreApp.Utils
{
    public class VietnameseTranslator
    {
        Dictionary<String, String> _dict;
        public VietnameseTranslator() { 
            _dict = new Dictionary<String, String>();
            _dict.Add("men", "nam");
            _dict.Add("women", "nữ");
            _dict.Add("unisex", "unisex");
            _dict.Add("basketball", "bóng rổ");
            _dict.Add("football", "bóng rổ");
            _dict.Add("black", "đen");
            _dict.Add("white", "trắng");
            _dict.Add("orange", "cam");
            _dict.Add("pink", "hồng");
            _dict.Add("red", "đỏ");
            _dict.Add("gray", "xám");
            _dict.Add("blue", "xanh dương");
            _dict.Add("green", "xanh lục");
        }
        public string Translate(string word)
        {
            return _dict[word];
        }
    }
}
