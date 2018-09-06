import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExperimentoverviewComponent } from './experimentoverview/experimentoverview.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DatasetoverviewComponent } from './datasetoverview/datasetoverview.component';
import { DatasetdetailsComponent } from './datasetdetails/datasetdetails.component';
import { AuthGuardService } from './auth-guard.service';
import { LoginComponent } from './login/login.component';
import { ExperimentViewComponent } from './experiment/experiment-view/experiment-view.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full',
    canActivate: [AuthGuardService]
  },
  {
    path: 'datasets',
    component: DatasetoverviewComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'datasets/:id',
    component: DatasetdetailsComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'experiments',
    component: ExperimentoverviewComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'experiments/:id',
    component: ExperimentViewComponent,
    canActivate: [AuthGuardService]
  },
  { path: 'login', component: LoginComponent }
  //{ path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
