﻿using System;
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
            var id = commodity.Id;
            var name = commodity.Name;
            var price = (decimal)commodity.Price;
            var likes = (int)commodity.Likes;
            var cover = commodity.Cover;
            var ownerId = commodity.OwnerId;
            var sellerAvt = commodity.Owner.Avatar.Path;
            
            var simplifiedCommodity = new SimplifiedCommodity(
                id,
                name,
                price,
                likes,
                cover,
                ownerId,
                sellerAvt
                );
            return simplifiedCommodity;
        }

        public static SimplifiedPost SimplfyPost(NeedPost post)
        {
            var simplifiedPost = new SimplifiedPost(post.Id, post.Title, post.Sender.Nickname, post.Content, post.SenderId,
                post.Sender.Avatar.Path);
            return simplifiedPost;
        }

        public static SimplifiedUser SimplifyUser(SchoolUser user)
        {
            var simplifiedUser = new SimplifiedUser(user.SchoolId, user.Nickname,
                user.Avatar.Path, user.RealName, user.Role);
            return simplifiedUser;
        }

        public static SimplifiedComment SimplifyComment(PostComment comment)
        {
            var simplifiedComment = new SimplifiedComment(comment.Id, comment.SenderId, comment.Sender.Nickname,
                comment.Sender.Avatar.Path, (DateTime)comment.Time, comment.Content);
            return simplifiedComment;
        }
    }
}