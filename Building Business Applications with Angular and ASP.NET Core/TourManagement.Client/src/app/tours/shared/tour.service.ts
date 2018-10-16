import { Injectable, ErrorHandler } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

import { Tour } from './tour.model';
import { TourWithEstimatedProfits } from './tour-with-estimated-profits.model';
import { BaseService } from '../../shared/base.service';
import { TourForCreation } from './tour-for-creation.model';
import { TourWithManagerForCreation } from './tour-with-manager-for-creation.model';
import { TourWithShows } from './tour-with-shows.model';
import { TourWithEstimatedProfitsAndShows } from './tour-with-estimated-profits-and-shows';

@Injectable()
export class TourService extends BaseService {

    constructor(private http: HttpClient) {           
        super();      
    }

    getTours(): Observable<Tour[]> {
        return this.http.get<Tour[]>(`${this.apiUrl}/tours`);
    }

    getTour(tourId: string): Observable<Tour> {
        return this.http.get<Tour>(`${this.apiUrl}/tours/${tourId}`);
    }

    getTourWithEstimatedProfits(tourId: string): Observable<TourWithEstimatedProfits> {
        return this.http.get<TourWithEstimatedProfits>(`${this.apiUrl}/tours/${tourId}`,
            { headers: {'Accept': 'application/vnd.marvin.tourwithestimatedprofits+json'} });
    }

    getTourWithShows(tourId: string): Observable<TourWithShows> {
        return this.http.get<TourWithShows>(`${this.apiUrl}/tours/${tourId}`,
            { headers: {'Accept': 'application/vnd.marvin.tourwithshows+json'} });    }

    getTourWithEstimatedProfitsAndShows(tourId: string): Observable<TourWithEstimatedProfitsAndShows> {
        return this.http.get<TourWithEstimatedProfitsAndShows>(`${this.apiUrl}/tours/${tourId}`,
            { headers: {'Accept': 'application/vnd.marvin.tourwithestimatedprofitsandshows+json'} });
    }

    addTour(tourToAdd: TourForCreation): Observable<Tour> {
        return this.http.post<Tour>(`${this.apiUrl}/tours`, tourToAdd,
            { headers: {'Content-Type': 'application/json'} });
    }

    addTourWithManager(tourToAdd: TourWithManagerForCreation): Observable<Tour> {
        return this.http.post<Tour>(`${this.apiUrl}/tours`, tourToAdd,
            { headers: {'Content-Type': 'application/vnd.marvin.tourwithmanagerforcreation+json'} });
    }
}
