import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ReminderEditComponent } from './components/reminder-edit/reminder-edit.component';
import { ReminderListComponent } from './components/reminder-list/reminder-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ReminderEditComponent,
    ReminderListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
