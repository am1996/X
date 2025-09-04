import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-post',
  standalone: true,
  imports: [FormsModule, HttpClientModule],
  templateUrl: './add-post.component.html',
  styleUrl: './add-post.component.css'
})
export class AddPostComponent {
  content: string = "";
  title: string = "";
  constructor(private http: HttpClient){}
  submit(){
    console.log(this.title);
    console.log(this.content);
    this.http.post("http://localhost:5118/api/post", {
      title: this.title,
      content: this.content
    },{
      withCredentials: true,
    }).subscribe({
      next: (response) => {
        alert("Post added successfully");
        window.location.href = "";
        this.title = "";
        this.content = "";
      },
      error: (error) => {
        console.error("Error adding post:", error);
        alert("Failed to add post: " + error.message);
      }
    });
  }
}
