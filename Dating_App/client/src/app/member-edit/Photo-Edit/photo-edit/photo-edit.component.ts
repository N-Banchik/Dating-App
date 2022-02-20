import { Component, Input, OnInit } from '@angular/core';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Member } from 'src/app/models/Members';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css'],
})
export class PhotoEditComponent implements OnInit {
  @Input() member: Member;

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;

  baseurl = environment.apiUrl;

  user: User;
  constructor(
    private accountservice: AccountService,
    private memberService: MembersService
  ) {
    this.accountservice.currentUser$.pipe(take(1)).subscribe((user) => {
      this.user = user as User;
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  initializeUploader() {
    const options: FileUploaderOptions = {
      url: `${this.baseurl}users/add-photo`,
      authToken: `Bearer ${this.user.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    };
    this.uploader = new FileUploader(options);

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    };
  }

  setMainPhoto(photo: any) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountservice.setCurrentUser(this.user);
      this.member.photos.forEach((p) => {
        p.isMain = p.id === photo.id;
      });
    });
  }

  deletePhoto(photo: any) {
    this.memberService.deletePhoto(photo).subscribe(() => {
      this.member.photos=this.member.photos.filter((p) => p.id !== photo);
    });
  }
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }
}
