using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class Comment
    {
        private bool approved;
        private int id;
        private Tourist commentator;
        private Arrangement commentedArrangement;
        private string commentText;
        private int grade;

        public Comment() { }
        
        public Comment(Tourist commentator, Arrangement commentedArrangement, string commentText, int grade)
        {
            this.Commentator = commentator;
            this.CommentedArrangement = commentedArrangement;
            this.CommentText = commentText;
            this.Grade = grade;
            this.Approved = false;
            this.Id = Data.comments.Count;
        }

        public Tourist Commentator { get => commentator; set => commentator = value; }
        public Arrangement CommentedArrangement { get => commentedArrangement; set => commentedArrangement = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public int Grade { get => grade; set => grade = value; }
        public bool Approved { get => approved; set => approved = value; }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5}", Id, approved, commentText, grade, commentator.Username, commentedArrangement.Id);
        }
    }
}