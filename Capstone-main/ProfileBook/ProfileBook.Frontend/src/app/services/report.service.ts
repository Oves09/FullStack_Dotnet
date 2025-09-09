import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Report, CreateReportRequest } from '../models/report.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private readonly API_URL = `${environment.apiUrl}/api/reports`;

  constructor(private http: HttpClient) {}

  createReport(request: CreateReportRequest): Observable<Report> {
    return this.http.post<Report>(this.API_URL, request);
  }

  getReport(id: number): Observable<Report> {
    return this.http.get<Report>(`${this.API_URL}/${id}`);
  }

  getMyReports(): Observable<Report[]> {
    return this.http.get<Report[]>(`${this.API_URL}/my-reports`);
  }
}
