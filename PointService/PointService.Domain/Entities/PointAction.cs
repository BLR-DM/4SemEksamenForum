﻿using System.ComponentModel.DataAnnotations;

namespace PointService.Domain.Entities
{
    public class PointAction
    {
        [Key]
        public string Action { get; protected set; }
        public int Points { get; protected set; }

        protected PointAction() { }

        private PointAction(string action, int points)
        {
            Action = action;
            Points = points;
        }

        public static PointAction Create(string action, int points)
        {
            return new PointAction(action, points);
        }

        public void UpdatePoints(int points)
        {
            Points = points;
        }

    }
}