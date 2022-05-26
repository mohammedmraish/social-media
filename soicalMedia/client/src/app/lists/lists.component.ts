import { MembersService } from './../_services/members.service';

import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Partial<Member[]>;
  predicate: string;

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadLiked();
  }

  loadLiked() {
    this.predicate = 'liked';

    this.memberService.getLikes(this.predicate).subscribe((res) => {
      this.members = res;
    });
  }

  loadLikedBy() {
    this.predicate = 'likedBy';

    this.memberService.getLikes(this.predicate).subscribe((res) => {
      this.members = res;
    });
  }
}
