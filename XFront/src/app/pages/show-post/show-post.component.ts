import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-show-post',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule,FormsModule],
  templateUrl: './show-post.component.html',
  styleUrls: ['./show-post.component.css']
})
export class ShowPostComponent {
  data: any;
  postId: any;
  addComment(){
    this.http.post(`http://localhost:5118/api/comment`,{
      PostId: this.postId,
      content: this.data.newComment
    },{
      withCredentials: true,
    }).subscribe({
      next: (response) => {
        alert("Post added successfully");
        window.location.href = "";
        this.data.newComment = "";
      },
      error: (error) => {
        console.error("Error adding post:", error);
        alert("Failed to add post: " + error.message);
      }
    });
  }
  ngOnInit(){
    this.route.paramMap.subscribe((params: { get: (arg0: string) => any; }) => {
      this.postId = params.get('id');
      this.http.get(`http://localhost:5118/api/post/${this.postId}`,{
        withCredentials: true
      }).subscribe({
        next: (response: any) => {
          this.data =response;
        },
        error: () => {
        }
      });
    });
  }

  constructor(private route: ActivatedRoute, private http: HttpClient) {
  }
}
