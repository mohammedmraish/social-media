import { NgForm } from '@angular/forms';
import { MassageService } from './../../_services/massage.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Massage } from 'src/app/_models/massage';

@Component({
  selector: 'app-member-massages',
  templateUrl: './member-massages.component.html',
  styleUrls: ['./member-massages.component.css'],
})
export class MemberMassagesComponent implements OnInit {
  @ViewChild('massageForm') form: NgForm;
  @Input() username: string;
  massages: Massage[];
  massageContent: string;

  constructor(private massageService: MassageService) {}

  ngOnInit(): void {
    this.loadMassages();
  }

  loadMassages() {
    this.massageService.getMassageThread(this.username).subscribe((res) => {
      this.massages = res;
      console.log(res);
    });
  }

  sendMassage() {
    this.massageService
      .sendMassage(this.username, this.massageContent)
      .subscribe((res) => {
        this.massages.push(res);
      });

    this.form.reset();
  }
}
