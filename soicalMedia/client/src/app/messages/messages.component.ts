import { MassageService } from './../_services/massage.service';
import { Pagination } from './../_models/pagination';
import { Component, OnInit } from '@angular/core';
import { Massage } from '../_models/massage';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  massages: Massage[];
  pagination: Pagination;
  container = 'Inbox';
  pageNumber = 1;
  pageSize = 5;

  constructor(private massageService: MassageService) {}

  ngOnInit(): void {
    this.loadMassages();
  }

  loadMassages(container = 'Outbox') {
    this.massageService
      .getMassages(this.pageNumber, this.pageSize, container)
      .subscribe((res) => {
        this.massages = res.result;
        this.pagination = res.pagination;
      });
  }

  deleteMassage(id: number) {
    console.log(id);

    this.massageService.deleteMassage(id).subscribe(() => {
      this.massages.splice(
        this.massages.findIndex((m) => m.id == id),
        1
      );
    });
  }

  pageChanged(event: any) {
    if (this.pageNumber != event.any) {
      this.pageNumber = event.page;
      this.loadMassages();
    }
  }
}
