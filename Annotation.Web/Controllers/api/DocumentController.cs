﻿using Annotation.Web.Models;
using Annotation.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api
{
    public class DocumentController : ApiController
    {
        public static List<DocumentModel> documents = new List<DocumentModel>();
        public DocumentController() {
            if (documents.Count == 0) {
                documents.Add(new DocumentModel() {
                    AnnotationCount = 10,
                    Owner = "Test1",
                    Title = "Title1",
                    Body = mockText,
                    Id = Guid.NewGuid()
                });
                documents.Add(new DocumentModel() {
                    AnnotationCount = 20,
                    Owner = "Test2",
                    Title = "Title2",
                    Body = mockText,
                    Id = Guid.NewGuid()
                });
            }
        }
        // GET: api/Document
        public IEnumerable<DocumentModel> Get() {
            return documents;
        }

        // GET: api/Document/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Document
        public DocumentModel Post([FromBody]DocumentModel doc) {
            documents.Add(doc);
            doc.Owner = IdentityUtil.GetCurrentUser().UserId;
            return doc;
        }

        // PUT: api/Document/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Document/5
        public void Delete(int id)
        {
        }

        string mockText = @"I INTRODUCTION IN DEFENCE OF EVERYTHING ELSE


     THE only possible excuse for this book is that it is an answer
to a challenge.  Even a bad shot is dignified when he accepts a duel.
When some time ago I published a series of hasty but sincere papers,
under the name of ""Heretics,"" several critics for whose intellect
I have a warm respect (I may mention specially Mr. G.S.Street)
said that it was all very well for me to tell everybody to affirm
his cosmic theory, but that I had carefully avoided supporting my
precepts with example.  ""I will begin to worry about my philosophy,""
said Mr. Street, ""when Mr. Chesterton has given us his.""
It was perhaps an incautious suggestion to make to a person
only too ready to write books upon the feeblest provocation.
But after all, though Mr. Street has inspired and created this book,
he need not read it.  If he does read it, he will find that in
its pages I have attempted in a vague and personal way, in a set
of mental pictures rather than in a series of deductions, to state
the philosophy in which I have come to believe.  I will not call it
my philosophy; for I did not make it.  God and humanity made it;
and it made me.

     I have often had a fancy for writing a romance about an English
yachtsman who slightly miscalculated his course and discovered England
under the impression that it was a new island in the South Seas.
I always find, however, that I am either too busy or too lazy to
write this fine work, so I may as well give it away for the purposes
of philosophical illustration.  There will probably be a general
impression that the man who landed (armed to the teeth and talking
by signs) to plant the British flag on that barbaric temple which
turned out to be the Pavilion at Brighton, felt rather a fool.
I am not here concerned to deny that he looked a fool.  But if you
imagine that he felt a fool, or at any rate that the sense of folly
was his sole or his dominant emotion, then you have not studied
with sufficient delicacy the rich romantic nature of the hero
of this tale.  His mistake was really a most enviable mistake;
and he knew it, if he was the man I take him for.  What could
be more delightful than to have in the same few minutes all the
fascinating terrors of going abroad combined with all the humane
security of coming home again?  What could be better than to have
all the fun of discovering South Africa without the disgusting
necessity of landing there?  What could be more glorious than to
brace one's self up to discover New South Wales and then realize,
with a gush of happy tears, that it was really old South Wales.
This at least seems to me the main problem for philosophers, and is
in a manner the main problem of this book.  How can we contrive
to be at once astonished at the world and yet at home in it?
How can this queer cosmic town, with its many-legged citizens,
with its monstrous and ancient lamps, how can this world give us
at once the fascination of a strange town and the comfort and honour
of being our own town?

     To show that a faith or a philosophy is true from every
standpoint would be too big an undertaking even for a much bigger
book than this; it is necessary to follow one path of argument;
and this is the path that I here propose to follow.  I wish to set
forth my faith as particularly answering this double spiritual need,
the need for that mixture of the familiar and the unfamiliar
which Christendom has rightly named romance.  For the very word
""romance"" has in it the mystery and ancient meaning of Rome.
Any one setting out to dispute anything ought always to begin by
saying what he does not dispute.  Beyond stating what he proposes
to prove he should always state what he does not propose to prove.
The thing I do not propose to prove, the thing I propose to take
as common ground between myself and any average reader, is this
desirability of an active and imaginative life, picturesque and full
of a poetical curiosity, a life such as western man at any rate always
seems to have desired.  If a man says that extinction is better
than existence or blank existence better than variety and adventure,
then he is not one of the ordinary people to whom I am talking.
If a man prefers nothing I can give him nothing.  But nearly all
people I have ever met in this western society in which I live
would agree to the general proposition that we need this life
of practical romance; the combination of something that is strange
with something that is secure.  We need so to view the world as to
combine an idea of wonder and an idea of welcome.  We need to be
happy in this wonderland without once being merely comfortable.
It is THIS achievement of my creed that I shall chiefly pursue in
these pages.

     But I have a peculiar reason for mentioning the man in
a yacht, who discovered England.  For I am that man in a yacht.
I discovered England.  I do not see how this book can avoid
being egotistical; and I do not quite see (to tell the truth)
how it can avoid being dull.  Dulness will, however, free me from
the charge which I most lament; the charge of being flippant.
Mere light sophistry is the thing that I happen to despise most of
all things, and it is perhaps a wholesome fact that this is the thing
of which I am generally accused.  I know nothing so contemptible
as a mere paradox; a mere ingenious defence of the indefensible.
If it were true (as has been said) that Mr. Bernard Shaw lived
upon paradox, then he ought to be a mere common millionaire;
for a man of his mental activity could invent a sophistry every
six minutes.  It is as easy as lying; because it is lying.
The truth is, of course, that Mr. Shaw is cruelly hampered by the
fact that he cannot tell any lie unless he thinks it is the truth.
I find myself under the same intolerable bondage.  I never in my life
said anything merely because I thought it funny; though of course,
I have had ordinary human vainglory, and may have thought it funny
because I had said it.  It is one thing to describe an interview
with a gorgon or a griffin, a creature who does not exist.
It is another thing to discover that the rhinoceros does exist
and then take pleasure in the fact that he looks as if he didn't.
One searches for truth, but it may be that one pursues instinctively
the more extraordinary truths.  And I offer this book with the
heartiest sentiments to all the jolly people who hate what I write,
and regard it (very justly, for all I know), as a piece of poor
clowning or a single tiresome joke.

     For if this book is a joke it is a joke against me.
I am the man who with the utmost daring discovered what had been
discovered before.  If there is an element of farce in what follows,
the farce is at my own expense; for this book explains how I fancied I
was the first to set foot in Brighton and then found I was the last.
It recounts my elephantine adventures in pursuit of the obvious.
No one can think my case more ludicrous than I think it myself;
no reader can accuse me here of trying to make a fool of him:
I am the fool of this story, and no rebel shall hurl me from
my throne.  I freely confess all the idiotic ambitions of the end
of the nineteenth century.  I did, like all other solemn little boys,
try to be in advance of the age.  Like them I tried to be some ten
minutes in advance of the truth.  And I found that I was eighteen
hundred years behind it.  I did strain my voice with a painfully
juvenile exaggeration in uttering my truths.  And I was punished
in the fittest and funniest way, for I have kept my truths:
but I have discovered, not that they were not truths, but simply that
they were not mine.  When I fancied that I stood alone I was really
in the ridiculous position of being backed up by all Christendom.
It may be, Heaven forgive me, that I did try to be original;
but I only succeeded in inventing all by myself an inferior copy
of the existing traditions of civilized religion.  The man from
the yacht thought he was the first to find England; I thought I was
the first to find Europe.  I did try to found a heresy of my own;
and when I had put the last touches to it, I discovered that it
was orthodoxy.

     It may be that somebody will be entertained by the account
of this happy fiasco.  It might amuse a friend or an enemy to
read how I gradually learnt from the truth of some stray legend
or from the falsehood of some dominant philosophy, things that I
might have learnt from my catechism--if I had ever learnt it.
There may or may not be some entertainment in reading how I
found at last in an anarchist club or a Babylonian temple what I
might have found in the nearest parish church.  If any one is
entertained by learning how the flowers of the field or the
phrases in an omnibus, the accidents of politics or the pains
of youth came together in a certain order to produce a certain
conviction of Christian orthodoxy, he may possibly read this book.
But there is in everything a reasonable division of labour.
I have written the book, and nothing on earth would induce me to read it.

     I add one purely pedantic note which comes, as a note
naturally should, at the beginning of the book.  These essays are
concerned only to discuss the actual fact that the central Christian
theology (sufficiently summarized in the Apostles' Creed) is the
best root of energy and sound ethics.  They are not intended
to discuss the very fascinating but quite different question
of what is the present seat of authority for the proclamation
of that creed.  When the word ""orthodoxy"" is used here it means
the Apostles' Creed, as understood by everybody calling himself
Christian until a very short time ago and the general historic
conduct of those who held such a creed.  I have been forced by
mere space to confine myself to what I have got from this creed;
I do not touch the matter much disputed among modern Christians,
of where we ourselves got it.  This is not an ecclesiastical treatise
but a sort of slovenly autobiography.  But if any one wants my
opinions about the actual nature of the authority, Mr. G.S.Street
has only to throw me another challenge, and I will write him another book.
";
    }
}
