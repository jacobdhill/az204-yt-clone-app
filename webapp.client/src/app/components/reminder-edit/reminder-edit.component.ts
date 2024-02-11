import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, map } from 'rxjs';

interface Result<T> {
  value: T;
  statusCode: number;
  contentType: string | null;
}

interface Reminder {
  reminderId: string;
  description: string;
  notifyDaysBefore: number;
  dayOfMonth: number;
}

@Component({
  selector: 'app-reminder-edit',
  templateUrl: './reminder-edit.component.html',
  styleUrl: './reminder-edit.component.css'
})
export class ReminderEditComponent {
  reminderId!: string;
  reminder!: Reminder;
  reminder$: Observable<Result<Reminder>> | undefined;
  reminderGroup: FormGroup;

  constructor(private httpClient: HttpClient, private fb: FormBuilder, private route: ActivatedRoute, private router: Router) {
    this.reminderGroup = this.fb.group({
      description: new FormControl('', Validators.required),
      dayOfMonth: new FormControl('', Validators.required),
      notifyDaysBefore: new FormControl('', Validators.required)
    });

    const reminderId = this.route.snapshot.paramMap.get('reminderId');
    if (reminderId == null) {
      this.router.navigateByUrl('/');
      return;
    }

    this.reminderId = reminderId;
    this.fetchReminder();
  }

  fetchReminder() {
    this.httpClient.get(`/api/reminder/${this.reminderId}`).pipe(map(result => result as Result<Reminder>)).subscribe({
      next: (result: Result<Reminder>) => {
        this.reminderGroup.get('description')?.setValue(result.value.description);
        this.reminderGroup.get('dayOfMonth')?.setValue(result.value.dayOfMonth);
        this.reminderGroup.get('notifyDaysBefore')?.setValue(result.value.notifyDaysBefore);
        this.reminderGroup.updateValueAndValidity();
      }
    });
  }

  editReminder() {
    this.httpClient.put(`/api/reminder/${this.reminderId}`, this.reminderGroup.value).subscribe().add(() => this.router.navigateByUrl('/'));
    this.reminderGroup.reset();
  }
}
