using Mercury_Backend.Models;

namespace Mercury_Backend.Utils
{
    public class Simplify
    {
        public static SimplifiedOrder SimplifyOrder(Order order)
        {
            var simplifiedOrder = new SimplifiedOrder(order.Id, order.BuyerId, (decimal)order.Commodity.Price
                , (int)order.Count, order.Status, order.Commodity.Cover);
            return simplifiedOrder;
        }

        public static SimplifiedCommodity SimplifyCommodity(Commodity commodity)
        {
            var simplifiedCommodity = new SimplifiedCommodity(commodity.Id, commodity.Name, (decimal) commodity.Price,
                (int) commodity.Likes, commodity.Cover, commodity.OwnerId, commodity.Owner.Avatar.Path);
            return simplifiedCommodity;
        }

        public static SimplifiedPost SimplfyPost(NeedPost post)
        {
            var simplifiedPost = new SimplifiedPost(post.Title, post.Sender.Nickname, post.Content, post.SenderId,
                post.Sender.Avatar.Path);
            return simplifiedPost;
        }
    }
}