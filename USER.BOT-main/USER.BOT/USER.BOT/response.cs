using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USER.BOT
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class FriendsGet
    {
        public Response response { get; set; }
        public class Item
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public bool is_closed { get; set; }
            public bool can_access_closed { get; set; }
            public string photo_50 { get; set; }
            public int online { get; set; }
            public string track_code { get; set; }
            public List<int> lists { get; set; }
        }

        public class Response
        {
            public int count { get; set; }
            public List<Item> items { get; set; }
        }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class UsersGet
    {
        public List<Response> response { get; set; }
        public class Response
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public bool is_closed { get; set; }
            public bool can_access_closed { get; set; }
            public string photo_50 { get; set; }
            public string photo_100 { get; set; }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class GetProfileInfo
    {
        public Response response { get; set; }
        public class City
        {
            public int id { get; set; }
            public string title { get; set; }
        }

        public class Country
        {
            public int id { get; set; }
            public string title { get; set; }
        }

        public class Response
        {
            public string first_name { get; set; }
            public int id { get; set; }
            public string last_name { get; set; }
            public string home_town { get; set; }
            public string status { get; set; }
            public string bdate { get; set; }
            public int bdate_visibility { get; set; }
            public City city { get; set; }
            public Country country { get; set; }
            public string phone { get; set; }
            public int relation { get; set; }
            public int sex { get; set; }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WallGet
    {
        public Response response { get; set; }

        public class PostSource
        {
            public string type { get; set; }
            public string platform { get; set; }
            public string data { get; set; }
        }

        public class Comments
        {
            public int count { get; set; }
            public int can_post { get; set; }
            public bool groups_can_post { get; set; }
            public int? can_close { get; set; }
        }

        public class Likes
        {
            public int count { get; set; }
            public int user_likes { get; set; }
            public int can_like { get; set; }
            public int can_publish { get; set; }
        }

        public class Reposts
        {
            public int count { get; set; }
            public int wall_count { get; set; }
            public int mail_count { get; set; }
            public int user_reposted { get; set; }
        }

        public class Size
        {
            public int height { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public int width { get; set; }
        }

        public class Photo
        {
            public int album_id { get; set; }
            public int date { get; set; }
            public int id { get; set; }
            public int owner_id { get; set; }
            public bool has_tags { get; set; }
            public string access_key { get; set; }
            public List<Size> sizes { get; set; }
            public string text { get; set; }
            public int? post_id { get; set; }
        }

        public class Attachment
        {
            public string type { get; set; }
            public Photo photo { get; set; }
        }

        public class Views
        {
            public int count { get; set; }
        }

        public class Item
        {
            public int id { get; set; }
            public int from_id { get; set; }
            public int owner_id { get; set; }
            public int date { get; set; }
            public string post_type { get; set; }
            public string text { get; set; }
            public int can_delete { get; set; }
            public bool can_archive { get; set; }
            public bool is_archived { get; set; }
            public PostSource post_source { get; set; }
            public Comments comments { get; set; }
            public Likes likes { get; set; }
            public Reposts reposts { get; set; }
            public bool is_favorite { get; set; }
            public int? can_pin { get; set; }
            public List<Attachment> attachments { get; set; }
            public Views views { get; set; }
            public int? edited { get; set; }
        }

        public class Response
        {
            public int count { get; set; }
            public List<Item> items { get; set; }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class CommentsGet
    {
        public Response response { get; set; }

        public class Thread
        {
            public int count { get; set; }
            public List<object> items { get; set; }
            public bool can_post { get; set; }
            public bool show_reply_button { get; set; }
            public bool groups_can_post { get; set; }
        }

        public class Item
        {
            public int id { get; set; }
            public int from_id { get; set; }
            public int post_id { get; set; }
            public int owner_id { get; set; }
            public List<object> parents_stack { get; set; }
            public int date { get; set; }
            public string text { get; set; }
            public Thread thread { get; set; }
        }

        public class Response
        {
            public int count { get; set; }
            public List<Item> items { get; set; }
            public int current_level_count { get; set; }
            public bool can_post { get; set; }
            public bool show_reply_button { get; set; }
            public bool groups_can_post { get; set; }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CommentsGet2
    {
        public Response response { get; set; }

        public class Item
        {
            public int id { get; set; }
            public int from_id { get; set; }
            public int post_id { get; set; }
            public int owner_id { get; set; }
            public List<int> parents_stack { get; set; }
            public int date { get; set; }
            public string text { get; set; }
        }

        public class Response
        {
            public int count { get; set; }
            public List<Item> items { get; set; }
            public int current_level_count { get; set; }
            public bool can_post { get; set; }
            public bool show_reply_button { get; set; }
            public bool groups_can_post { get; set; }
        }
    }
}
