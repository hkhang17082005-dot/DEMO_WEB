using System;
using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities;

public class CV
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string UserId { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string Status { get; set; } = "Bản nháp";
}