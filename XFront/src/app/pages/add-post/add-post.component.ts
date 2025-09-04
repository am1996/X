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
    this.title = "";
    this.content = "";
  }
}
