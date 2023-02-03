namespace StreamingTweetsLibrary.Models
{
    public class SingleTweet
    {

        public string[] edit_history_tweet_ids { get; set; } = default!;
        public string id { get; set; } = default!;
        public string text { get; set; } = default!;

        //{
        // "data": {
        //  "edit_history_tweet_ids": [
        //   "1621394639029932034"
        //  ],
        //  "id": "1621394639029932034",
        //  "text": "RT @1992Earthh: หลายคนห่วงจองกุกว่าน้องไม่ทำอะไรเลยเป็นห่วงจัง สำหรับนี่มันคือการพักแบบเต็มที่อะ น้องเองก็พูดเองชอบ คือไม่อยากให้ห่วงมากเกิ…"
        // }
        //}
    }
}
