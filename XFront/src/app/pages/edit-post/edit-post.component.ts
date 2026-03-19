import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-post',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']
})
export class EditPostComponent implements OnInit{
  postId: string | null = null;
  public data: any | undefined;
  
  editPost(){
    this.http.put(`http://localhost:5118/api/post/${this.postId}`,this.data,{
      withCredentials: true
    }).subscribe({
      next: (response) => {
        alert("Post Edited Successfully");
      }
    });
  }

  ngOnInit(){
    this.route.paramMap.subscribe(params => {
      this.postId = params.get('id');
      this.http.get(`http://localhost:5118/api/post/${this.postId}`,{
        withCredentials: true
      }).subscribe({
        next: (response) => {
          this.data =response;
        },
        error: (error) => {
        }
      });
    });
  }

  constructor(private route: ActivatedRoute, private http: HttpClient) {
  }

}
