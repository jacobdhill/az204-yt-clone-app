import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoListComponent } from './components/video-list/video-list.component';
import { VideoViewComponent } from './components/video-view/video-view.component';
import { VideoNewComponent } from './components/video-new/video-new.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'videos',
  },
  {
    path: 'videos',
    component: VideoListComponent
  },
  {
    path: 'videos/new',
    pathMatch: 'full',
    component: VideoNewComponent
  },
  {
    path: 'videos/:videoId',
    component: VideoViewComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
