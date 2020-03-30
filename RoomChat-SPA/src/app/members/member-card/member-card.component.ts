import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  @Input() connectionsParam: string;
  @Output() send: EventEmitter<any> = new EventEmitter();

  constructor(private authService: AuthService,
              private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendConnection(id: number) {
    this.userService.sendConnectionRequest(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You have sent connection request to ' + this.user.displayName);
      this.send.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

}
