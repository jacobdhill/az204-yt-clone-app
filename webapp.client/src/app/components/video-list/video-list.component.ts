import { Component, OnInit } from '@angular/core';
import { Observable, map } from 'rxjs';
import { VideoListDto } from '../../models/ModelDtos';
import { HttpClient } from '@angular/common/http';
import { Result } from '../../models/Model';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-video-list',
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.css',
})
export class VideoListComponent implements OnInit {
  items: MenuItem[] = [{
    label: 'Videos',
    routerLink: '/videos'
  }];
  videos$: Observable<VideoListDto[]> | undefined;

  constructor(private httpClient: HttpClient) {

  }

  ngOnInit(): void {
    this.videos$ = this.httpClient.get('/api/video').pipe(map(result => result as Result<VideoListDto[]>), map(result => result.value));
  }
}
