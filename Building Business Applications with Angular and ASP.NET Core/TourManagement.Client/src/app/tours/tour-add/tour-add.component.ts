import { Component, OnInit, OnDestroy } from '@angular/core';
import { Band } from '../../shared/band.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { MasterDataService } from '../../shared/master-data.service';
import { TourService } from '../shared/tour.service';
import { Router } from '@angular/router';
import { Manager } from '../../shared/manager.model';

@Component({
  selector: 'app-tour-add',
  templateUrl: './tour-add.component.html',
  styleUrls: ['./tour-add.component.css']
})
export class TourAddComponent implements OnInit {

  public tourForm: FormGroup;
  bands: Band[];
  managers: Manager[];
  private isAdmin: boolean = true;

  constructor(private masterDataService: MasterDataService,
    private tourService: TourService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit() {

    // define the tourForm (with empty default values)
    this.tourForm = this.formBuilder.group({
      band: [''],
      manager: [''],
      title: [''],
      description: [''],
      startDate: [],
      endDate: []
    });

    // get bands from master data service
    this.masterDataService.getBands()
      .subscribe(bands => {
        this.bands = bands;
      });    
    
    if (this.isAdmin) {
      // get managers from master data service
      this.masterDataService.getManagers()
      .subscribe(managers => {
        this.managers = managers;
      });    
    }
  }

  addTour(): void {
    if (this.tourForm.dirty) {
      // assign value
      if (this.isAdmin) {
        // create TourWithManagerForCreation from form model
        let tour = automapper.map('TourFormModel', 'TourWithManagerForCreation', this.tourForm.value);

        this.tourService.addTourWithManager(tour)
          .subscribe( () => {
            this.router.navigateByUrl('/tours');
        });    
      } else {
        // create TourForCreation from form model
        let tour = automapper.map('TourFormModel', 'TourForCreation', this.tourForm.value);

        this.tourService.addTour(tour)
          .subscribe( () => {
            this.router.navigateByUrl('/tours');
        });
      }
    }
  }

}
