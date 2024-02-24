import { Component, OnInit } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { Result } from '../../models/Model';
import { VideoListDto, VideoReadDto } from '../../models/ModelDtos';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';

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
  video$: Observable<VideoReadDto> | undefined;
  videos$: Observable<VideoListDto[]> | undefined;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => {
      this.items = [{
        label: 'Videos',
        routerLink: '/videos'
      }];

      const videoId = paramMap.get('videoId');
      this.video$ = this.httpClient.get(`/api/video/${videoId}`).pipe(map(result => result as Result<VideoReadDto>), map(result => result.value), tap(video => this.items.push({
        label: video.title,
      })));

      this.videos$ = this.httpClient.get('/api/video').pipe(map(result => result as Result<VideoListDto[]>), map(result => result.value.filter(video => video.id != videoId && video.thumbnailUrl != null)));
    });
  }
}
