import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { ExperimentoverviewComponent } from './experimentoverview/experimentoverview.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ExperimentcontrollerService } from './experimentcontroller.service';
import { MaterialModule } from './material.module';
import { ExperimentcreationdialogComponent } from './dialogs/experimentcreationdialog/experimentcreationdialog.component';
import { DeleteconfirmationdialogComponent } from './dialogs/deleteconfirmationdialog/deleteconfirmationdialog.component';
import { AppRoutingModule } from './app-routing.module';
import { ParameterEditorComponent } from './controls/parameter-editor/parameter-editor.component';
import { ParameterTypePipe } from './controls/parameter-editor/parametertype-pipe';
import { ExperimentstateService } from './experimentstate.service';
import { DatasetoverviewComponent } from './datasetoverview/datasetoverview.component';
import { DatasetdetailsComponent } from './datasetdetails/datasetdetails.component';
import { InputParameterDialogComponent } from './dialogs/input-parameter-dialog/input-parameter-dialog.component';
import { TasksetviewComponent } from './controls/tasksetview.component';
import { AuthGuardService } from './auth-guard.service';
import { OAuthModule } from 'angular-oauth2-oidc';
import { LoginComponent } from './login/login.component';
import { TokenInterceptor } from './authentication-interceptor.service';
import { ExperimentModule } from './experiment/experiment.module';
import { ExperimentApiService } from './experiment/experiment-api.service';
import { StringInputDialogComponent } from './dialogs/string-input-dialog/string-input-dialog.component';
import { DatasetListService } from './dataset-list.service';
import { UtilsModule } from './utils/utils.module';

@NgModule({
  declarations: [
    AppComponent,
    ExperimentoverviewComponent,
    DashboardComponent,
    ExperimentcreationdialogComponent,
    DeleteconfirmationdialogComponent,
    ParameterEditorComponent,
    ParameterTypePipe,
    DatasetoverviewComponent,
    DatasetdetailsComponent,
    InputParameterDialogComponent,
    TasksetviewComponent,
    LoginComponent,
    StringInputDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    AppRoutingModule,
    OAuthModule.forRoot(),
    ExperimentModule,
    UtilsModule
  ],
  providers: [
    ExperimentcontrollerService,
    ExperimentstateService,
    AuthGuardService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    ExperimentApiService,
    DatasetListService
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    ExperimentcreationdialogComponent,
    DeleteconfirmationdialogComponent,
    InputParameterDialogComponent,
    StringInputDialogComponent
  ]
})
export class AppModule {}
