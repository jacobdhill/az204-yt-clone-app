import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReminderEditComponent } from './components/reminder-edit/reminder-edit.component';
import { ReminderListComponent } from './components/reminder-list/reminder-list.component';

const routes: Routes = [
  {
    path: '',
    component: ReminderListComponent,
  },
  {
    path: 'reminder/:reminderId',
    component: ReminderEditComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
