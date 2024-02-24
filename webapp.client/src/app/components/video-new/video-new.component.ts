import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { FileUploadEvent, FileUploadHandlerEvent } from 'primeng/fileupload';
import { Result } from '../../models/Model';
import { map, tap } from 'rxjs';

@Component({
  selector: 'app-video-new',
  templateUrl: './video-new.component.html',
  styleUrl: './video-new.component.css'
})
export class VideoNewComponent {
  formGroup: FormGroup;
  items: MenuItem[] = [{
    label: 'Videos',
    routerLink: '/videos'
  }, {
    label: 'New',
  }];

  get tags() {
    return this.formGroup.get('tags') as FormArray;
  }

  constructor(private fb: FormBuilder, private httpClient: HttpClient, private router: Router) {
    this.formGroup = this.fb.group({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      tags: this.fb.array([new FormControl('', Validators.required)], Validators.minLength(1)),
      fileName: new FormControl('', Validators.required),
    });
  }

  onUpload(event: FileUploadEvent) {
    const file = event.files[0];
    this.formGroup.patchValue({ file: file });
    this.formGroup.updateValueAndValidity();
  }

  handleUpload(event: FileUploadHandlerEvent) {
    const file = event.files[0];
    const formData = new FormData();
    formData.append('file', file);

    this.httpClient.post('/api/video/upload', formData)
      .pipe(map(result => result as Result<string>),
        tap(result => this.formGroup.patchValue({ fileName: result.value })))
      .subscribe();
  }

  onCreate() {
    this.httpClient.post('/api/video', this.formGroup.value)
      .pipe(map(result => result as Result<string>), tap(result => this.router.navigateByUrl(`/videos/${result.value}`)))
      .subscribe();
  }

  addTag() {
    this.tags.push(new FormControl('', Validators.required));
  }

  removeTag(index: number) {
    this.tags.removeAt(index);
  }
}
