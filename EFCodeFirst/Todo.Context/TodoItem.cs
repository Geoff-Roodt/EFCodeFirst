﻿using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Todo.Context
{
    [DataContract]
    public class TodoItem
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [MaxLength(254)]
        public string Description { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }
    }
}