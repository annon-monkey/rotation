using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class CommentElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Comment;

        public string Comment
        {
            get => this._comment;
            set
            {
                if (this._comment != value)
                {
                    this._comment = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private string _comment;

        public CommentElement()
        {
            this.IsComment = true;
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"{this.Comment}";
        }

        public override IEntity ToEntity(IEntity before)
        {
            return before;
        }
    }
}
