import { Component, OnInit, Signal, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-edit-post',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']
})
export class EditPostComponent implements OnInit{
  postId: string | null = null;
  public data: any | undefined;

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
