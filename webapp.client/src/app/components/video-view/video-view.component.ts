import { Component, OnInit } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { Result } from '../../models/Model';
import { ListCommentDto, ListVideoDto, ReadVideoDto } from '../../models/ModelDtos';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-video-view',
  templateUrl: './video-view.component.html',
  styleUrl: './video-view.component.css'
})
export class VideoViewComponent implements OnInit {
  items: MenuItem[] = [{
    label: 'Videos',
    routerLink: '/videos'
  }];
  videoId: string = '';
  video$: Observable<ReadVideoDto> | undefined;
  videos$: Observable<ListVideoDto[]> | undefined;

  comments: ListCommentDto[] = [];
  commentControl: FormControl | undefined;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => {
      this.items = [{
        label: 'Videos',
        routerLink: '/videos'
      }];

      this.videoId = paramMap.get('videoId')!;
      this.video$ = this.httpClient.get(`/api/video/${this.videoId}`).pipe(map(result => result as Result<ReadVideoDto>), map(result => result.value), tap(video => this.items.push({
        label: video.title,
      })));

      this.videos$ = this.httpClient.get('/api/video').pipe(map(result => result as Result<ListVideoDto[]>), map(result => result.value.filter(video => video.id != this.videoId && video.thumbnailUrl != null)));

      this.fetchComments();
    });
  }

  fetchComments() {
    this.httpClient.get(`/api/video/${this.videoId}/comments`).pipe(map(result => result as Result<ListCommentDto[]>), map(result => result.value), tap(comments => this.comments = comments)).subscribe();
  }

  addComment() {
    this.commentControl = new FormControl('', Validators.required);
  }

  cancelComment() {
    this.commentControl = undefined;
  }

  postComment() {
    this.httpClient.post(`/api/video/${this.videoId}/comments`, {
      message: this.commentControl?.value,
      videoId: this.videoId
    })
      .subscribe()
      .add(() => { this.commentControl = undefined, this.fetchComments(); });
  }

  likeComment(commentId: string) {
    this.httpClient.post(`/api/video/${this.videoId}/comments/${commentId}/like`, {}).subscribe().add(() => this.fetchComments());
  }
}
