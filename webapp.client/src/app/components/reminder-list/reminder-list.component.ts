import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
  selector: 'app-reminder-list',
  templateUrl: './reminder-list.component.html',
  styleUrl: './reminder-list.component.css'
})
export class ReminderListComponent {
  reminders$: Observable<Result<Reminder[]>> | undefined;
  reminderGroup: FormGroup;

  constructor(private httpClient: HttpClient, private fb: FormBuilder) {
    this.reminderGroup = this.fb.group({
      description: new FormControl('', Validators.required),
      dayOfMonth: new FormControl('', Validators.required),
      notifyDaysBefore: new FormControl('', Validators.required)
    });

    this.fetchReminders();
  }

  fetchReminders() {
    this.reminders$ = this.httpClient.get('/api/reminder').pipe(map(result => result as Result<Reminder[]>));
  }

  saveReminder() {
    this.httpClient.post('/api/reminder', this.reminderGroup.value).subscribe().add(() => this.fetchReminders());
    this.reminderGroup.reset();
  }

  deleteReminder(reminderId: string) {
    this.httpClient.delete(`/api/reminder/${reminderId}`).subscribe().add(() => this.fetchReminders());
  }
}
