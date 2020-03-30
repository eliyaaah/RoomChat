import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  userParams: any = {};
  pagination: Pagination;
  companies: string[];
  locations: string[];

  constructor(private userService: UserService,
              private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.userService.getCompanyList().subscribe((res: string[]) => {
      this.companies = res;
    }, error => {
      this.alertify.error(error);
    });

    this.userService.getLocationList().subscribe((res: string[]) => {
      this.locations = res;
    }, error => {
      this.alertify.error(error);
    });

    this.userParams.company = '';
    this.userParams.location = '';
    this.userParams.orderBy = 'lastActive';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters() {
    // this.userParams.company = this.user.company;
    // this.userParams.location = this.user.location;
    this.userParams.company = '';
    this.userParams.location = '';
    this.loadUsers();
  }

  companyChanged(company: string) {
    this.userParams.company = company;
    this.loadUsers();
  }

  locationChanged(location: string) {
    this.userParams.location = location;
    this.loadUsers();
  }


  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
